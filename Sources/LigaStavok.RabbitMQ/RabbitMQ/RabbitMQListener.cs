using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitMQListener : IDisposable, IAsyncEnumerable<ReadOnlyMemory<byte>>
    {
        private readonly BufferBlock<ReadOnlyMemory<byte>> bufferBlock;

        private readonly ILogger logger;
        private readonly RabbitClientOptions options;
        private readonly ConnectionFactory connectionFactory;
        private CancellationTokenSource cancellationTokenSource;
        private IConnection rabbitConnection;
        private IModel rabbitModel;

        public RabbitMQListener(ILogger<RabbitMQListener> logger, IOptions<RabbitClientOptions> rabbitOptions)
        {
            this.logger = logger;
            options = rabbitOptions.Value;

            // Factory
            connectionFactory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
                NetworkRecoveryInterval = options.NetworkRecoveryInterval,
                HostName = options.Host,
                Password = options.Password,
                Port = options.Port,
                Ssl = new SslOption
                {
                    Enabled = options.SslEnabled,
                    AcceptablePolicyErrors = options.AcceptablePolicyErrors
                },
                UserName = options.UserName,
                VirtualHost = options.VirtualHost
            };

            // Buffer
            bufferBlock
                = new BufferBlock<ReadOnlyMemory<byte>>(
                    new DataflowBlockOptions()
                    {
                        EnsureOrdered = options.EnsureOrdered
                    }
                 ) ;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Initializing new connection
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => StartConnectionScheduler(cancellationTokenSource.Token, TimeSpan.Zero));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if ((!cancellationTokenSource?.IsCancellationRequested) ?? false)
                cancellationTokenSource.Cancel();

            rabbitConnection?.Dispose();
            rabbitModel?.Dispose();

            return Task.CompletedTask;
        }

        private async Task StartConnectionScheduler(CancellationToken cancelationToken, TimeSpan initilizeDelay)
        {
            if (initilizeDelay.TotalSeconds > 0)
                logger.LogDebug("FeedConnectionDelay {Seconds} sec...", initilizeDelay.TotalSeconds);

            await Task.Delay(initilizeDelay);

            while (!cancelationToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogDebug("{Action} {Endpoint}", "FeedConnectionInitializing...",
                        $"{options.Host}:{options.Port}/{options.VirtualHost}");

                    StartConnection();

                    logger.LogInformation("{Action}  {Endpoint}", "FeedConnectionStarted",
                        $"{options.Host}:{options.Port}/{options.VirtualHost}");

                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "StartConnectionError");
                }

                await Task.Delay(options.ReconnectionTimeout, cancelationToken);
            }
        }

        private void StartConnection()
        {
            rabbitConnection = connectionFactory.CreateConnection();
            rabbitConnection.ConnectionShutdown += (sender, args) => ConnectionOnConnectionShutdown(sender, args);

            rabbitModel = rabbitConnection.CreateModel();

            var queueName = rabbitModel.QueueDeclare(options.Queue, options.Durable, options.Exclusive, options.AutoDelete);

            var consumer = new EventingBasicConsumer(rabbitModel);
            consumer.Received += (o, args) => ConsumerOnReceived(o, args);

            rabbitModel.BasicConsume(queueName, true, consumer);

            if (!string.IsNullOrWhiteSpace(options.Exchange))
            {
                foreach (var key in options.RoutingKeys)
                    rabbitModel.QueueBind(queueName, options.Exchange, key);
            }
        }

        private void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogWarning("{Action}:{Reason}", "FeedConnectionStopped", e.ReplyText);

            // Disposing old objects
            rabbitConnection.Dispose();
            rabbitModel?.Dispose();

            // Initializing new connection
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => StartConnectionScheduler(cancellationTokenSource.Token, options.AfterConnectionDownTimeout));
        }

        private void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
                try
                {
                    bufferBlock.Post(e.Body);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "SendingToManager");
                }
        }

        public async IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(CancellationToken token = default)
        {
            // Return new elements until cancellationToken is triggered.
            while (true)
            {
                // Make sure to throw on cancellation so the Task will transfer into a canceled state
                token.ThrowIfCancellationRequested();
                yield return await bufferBlock.ReceiveAsync(token);
            }
        }

        public IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target)
		{
            return bufferBlock.LinkTo(target);
		}

        public IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target, DataflowLinkOptions linkOptions)
        {
            return bufferBlock.LinkTo(target, linkOptions);
        }

        public override string ToString()
		{
            return bufferBlock.ToString();
		}
        public bool TryReceive(Predicate<ReadOnlyMemory<byte>> filter, out ReadOnlyMemory<byte> item)
		{
            return bufferBlock.TryReceive(filter, out item);
		}
        public bool TryReceiveAll(out IList<ReadOnlyMemory<byte>> items)
		{
            return bufferBlock.TryReceiveAll(out items);
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if ((!cancellationTokenSource?.IsCancellationRequested) ?? false)
                        cancellationTokenSource.Cancel();

                    rabbitConnection?.Dispose();
                    rabbitModel?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RabbitManager()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
