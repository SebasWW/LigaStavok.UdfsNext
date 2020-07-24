using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitListener : IDisposable, IRabbitListener
	{
		private readonly RabbitClientOptions options;
		private readonly ConnectionFactory connectionFactory;

		private CancellationTokenSource cancellationTokenSource;
		private IConnection rabbitConnection;
		private IModel rabbitModel;

		public RabbitListener(IOptions<RabbitClientOptions> rabbitOptions)
		{
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
				OnConnectionStartingDelay(initilizeDelay);

			await Task.Delay(initilizeDelay);

			while (!cancelationToken.IsCancellationRequested)
			{
				try
				{
					OnConnecting(new ConnectingEventArgs { Uri = new Uri($"amqp://{options.Host}:{options.Port}/{options.VirtualHost}") });
					StartConnection();
					OnConnected();

					return;
				}
				catch (Exception ex)
				{
					OnConnectionStartingError(ex);
				}

				await Task.Delay(options.ReconnectionTimeout, cancelationToken);
			}
		}

		private void StartConnection()
		{
			rabbitConnection = connectionFactory.CreateConnection();
			rabbitConnection.ConnectionShutdown += (sender, args) => ConnectionShutdown(sender, args);

			rabbitModel = rabbitConnection.CreateModel();

			var queueDeclareOk = rabbitModel.QueueDeclare(options.Queue, options.Durable, options.Exclusive, options.AutoDelete);

			var consumer = new EventingBasicConsumer(rabbitModel);
			consumer.Received += (o, args) => OnMessageReceived(o, args);

			rabbitModel.BasicConsume(queueDeclareOk, true, consumer);

			if (!string.IsNullOrWhiteSpace(options.Exchange))
			{
				foreach (var key in options.RoutingKeys)
					rabbitModel.QueueBind(queueDeclareOk, options.Exchange, key);
			}
		}

		public virtual void OnConnected() { }

		public virtual void OnConnecting(ConnectingEventArgs connectionStartingEventArgs) { }

		public virtual void OnConnectionStartingError(Exception ex) { }

		public virtual void OnConnectionStartingDelay(TimeSpan initilizeDelay) { }

		public virtual void OnConnectionShutdown(ShutdownEventArgs e) { }

		private void ConnectionShutdown(object sender, ShutdownEventArgs e)
		{
			OnConnectionShutdown(e);

			// Disposing old objects
			rabbitConnection.Dispose();
			rabbitModel?.Dispose();

			// Initializing new connection
			cancellationTokenSource = new CancellationTokenSource();
			Task.Run(() => StartConnectionScheduler(cancellationTokenSource.Token, options.AfterConnectionDownTimeout));
		}

		public delegate void OnMessageReceivedHandler(object sender, BasicDeliverEventArgs e);
		public event EventHandler OnMessageReceivedEvent;

		public virtual void OnMessageReceived(object sender, BasicDeliverEventArgs e)
		{
			OnMessageReceivedEvent.Invoke(this, e);
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
