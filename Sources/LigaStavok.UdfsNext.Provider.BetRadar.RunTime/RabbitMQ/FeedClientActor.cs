//using System;
//using System.Collections.Immutable;
//using System.Linq;
//using System.Net.Security;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Akka.Actor;
//using NLog;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
//using Udfs.BetradarUnifiedFeed.Plugin.Adapter;
//using Udfs.BetradarUnifiedFeed.Plugin.Adapter.Extensions;
//using Udfs.BetradarUnifiedFeed.Plugin.Client;
//using Udfs.BetradarUnifiedFeed.Plugin.Db.Repositories;
//using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor;
//using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor.Messages;
//using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
//using Udfs.BetradarUnifiedFeed.Plugin.InputDumping;
//using Udfs.BetradarUnifiedFeed.Plugin.InputDumping.Messages;
//using Udfs.BetradarUnifiedFeed.Plugin.RabbitDumping;
//using Udfs.Common.Actor;
//using Udfs.Common.Messages;
//using Udfs.Transmitter.Messages.Identifiers;

//namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
//{
//    public class FeedClientActor : BetradarUnifiedActorBase
//    {
//        private readonly FeedConfiguration _feedConfiguration;
//        private readonly IFeedMessageParser _feedMessageParser;
//		private readonly SettingsRepository settingsRepository;
//		private readonly ProductType[] _products;
//        private const SslPolicyErrors AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNotAvailable
//                                                               | SslPolicyErrors.RemoteCertificateNameMismatch
//                                                               | SslPolicyErrors.RemoteCertificateChainErrors;

//        private readonly Logger _logger;
//        private bool isConnected = false;
//        private CancellationTokenSource cancellationTokenSource;
//        private IConnection _connection;
//        private IModel _channel;
//        private QueueDeclareOk _queueName;
//        private readonly ICanTell _dumpSender;
//        private readonly ICanTell _failureActor;
//        private readonly ICanTell _inputDumper;
//        private readonly ICanTell _adapterActor;

//        public FeedClientActor(
//            LogFactory logFactory,
//            FeedConfiguration feedConfiguration,
//            ClientConfiguration clientConfiguration,
//            IFeedMessageParser feedMessageParser,
//            ActorMetadata<RabbitDumpSenderActor> dumpSenderActorMetadata,
//            ActorMetadata<FailureSupervisorActor> failureActorMeta,
//            ActorMetadata<AdapterActor> adapterActorMeta,
//            ActorMetadata<InputMessageDumperActor> inputDumperActorMetadata,
//            SettingsRepository settingsRepository
//        ) : base(logFactory)
//        {
//            _feedConfiguration = feedConfiguration;
//            _feedMessageParser = feedMessageParser;
//			this.settingsRepository = settingsRepository;
//			_products = clientConfiguration.Products.Select(ParseProduct).ToArray();

//            _logger = logFactory.GetCurrentClassLogger();

//            _dumpSender = Context.ActorSelection(dumpSenderActorMetadata);
//            _failureActor = Context.ActorSelection(failureActorMeta);
//            _inputDumper = Context.ActorSelection(inputDumperActorMetadata);
//            _adapterActor = Context.ActorSelection(adapterActorMeta);

//            Receive<RawFeedMessage>((msg) => OnRawFeedMessage(msg));
//            Receive<ShutdownCommand>((msg) => OnShutdownCommand(msg));



//            // Initializing new connection
//            var self = Self;
//            cancellationTokenSource = new CancellationTokenSource();
//            Task.Run(() => StartConnectionScheduler(cancellationTokenSource.Token, self, TimeSpan.Zero));

//        }

//        private void OnRawFeedMessage(RawFeedMessage message)
//        {
//            SendMessageDump(message);
//            ProcessRawMessage(message);
//        }

//        private void OnShutdownCommand(ShutdownCommand message)
//        {
//            Context.Stop(Self);
//        }

//        private void SendMessageDump(RawFeedMessage rawFeedMessage)
//        {
//            var messageDump = new BetradarMessageDump(
//                incomingId: rawFeedMessage.IncomingId.ToString(),
//                receivedOn: rawFeedMessage.ReceivedOn.UtcDateTime,
//                body: rawFeedMessage.Data);

//            _dumpSender.Tell(messageDump, Self);
//        }

//        private async Task StartConnectionScheduler(CancellationToken cancelationToken, IActorRef actorRef, TimeSpan initilizeDelay)
//        {
//            if (initilizeDelay.TotalSeconds > 0)
//            _logger.Info("FeedConnectionDelay {Seconds} sec...", initilizeDelay.TotalSeconds);

//            await Task.Delay(initilizeDelay);

//            while (!cancelationToken.IsCancellationRequested)
//            {
//                try
//                {
//                    _logger.Info("{Action} {Endpoint}", "FeedConnectionInitializing...",
//                        $"{_feedConfiguration.Host}:{_feedConfiguration.Port}{_feedConfiguration.VirtualHost}");
                    
//                     await StartConnection(actorRef);

//                    _logger.Info("{Action} {Endpoint}", "FeedConnectionStarted",
//                        $"{_feedConfiguration.Host}:{_feedConfiguration.Port}{_feedConfiguration.VirtualHost}");

//                    return;
//                }
//                catch (Exception ex)
//                {
//                    _logger.Error(ex, "StartConnectionError");
//                }

//                await Task.Delay(TimeSpan.FromSeconds(_feedConfiguration.ReconnectionTimeoutSeconds), cancelationToken);
//            }
//        }

//        private async Task StartConnection(IActorRef actorRef)
//        {
//            if (!isConnected)
//            {
//                var connectionFactory = new ConnectionFactory
//                    {
//                        AutomaticRecoveryEnabled = true,
//                        NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
//                        HostName = _feedConfiguration.Host,
//                        Password = _feedConfiguration.Password,
//                        Port = _feedConfiguration.Port,
//                        Ssl = new SslOption
//                        {
//                            Enabled = _feedConfiguration.Ssl,
//                            AcceptablePolicyErrors = AcceptablePolicyErrors
//                        },
//                        UserName = _feedConfiguration.UserName,
//                        VirtualHost = _feedConfiguration.VirtualHost
//                    };

//                _connection = connectionFactory.CreateConnection();

//                if (await settingsRepository.GetControlParams(BetradarUnifiedFeed.PluginName + "_ActiveNode_" + System.Net.Dns.GetHostName()) != "1")
//                {
//                    _logger.Warn("Passive mode is detected. Amqp connection is checked.");

//                    _connection.Close();
//                    return;
//                }

//                _connection.ConnectionShutdown += (sender, args) => ConnectionOnConnectionShutdown(sender, args, actorRef);
//                _channel = _connection.CreateModel();
//                _queueName = _channel.QueueDeclare(_feedConfiguration.Queue, _feedConfiguration.Durable, _feedConfiguration.Exclusive, _feedConfiguration.AutoDelete);

//                var consumer = new EventingBasicConsumer(_channel);
//                consumer.Received += (o, args) => ConsumerOnReceived(o, args, actorRef);
//                _channel.BasicConsume(_queueName, true, consumer);

//                if (!string.IsNullOrWhiteSpace(_feedConfiguration.Exchange))
//                {
//                    foreach (var key in _feedConfiguration.RoutingKeys)
//                        _channel.QueueBind(_queueName, _feedConfiguration.Exchange, key);
//                }

//                isConnected = true;;
//            }
//            else
//            {
//                throw new InvalidOperationException($"{nameof(FeedClientActor)} is already initialized.");
//            }
//        }

//        private void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs e, IActorRef actorRef)
//        {
//            _logger.Info("{Action}:{Reason}", "FeedConnectionStopped", e.ReplyText);
//            isConnected = false;

//            // Disposing old objects
//            _connection.Dispose();
//            _channel.Dispose();

//            // Initializing new connection
//            cancellationTokenSource = new CancellationTokenSource();
//            Task.Run(() => StartConnectionScheduler(cancellationTokenSource.Token, actorRef, TimeSpan.FromSeconds(_feedConfiguration.AfterConnectionDownTimeoutSeconds)));
//        }

//        private void ConsumerOnReceived(object sender, BasicDeliverEventArgs e, IActorRef self)
//        {
//            var feedMessage = new RawFeedMessage(
//                incomingId: Guid.NewGuid(),
//                receivedOn: DateTimeOffset.UtcNow,
//                data: e.Body.ToImmutableArray()
//            );

//            self.Tell(feedMessage);
//        }

//        protected virtual void ProcessRawMessage(RawFeedMessage message)
//        {
//            IFeedMessageParsingResult parsedMessage = null;

//            try
//            {
//                if (!TryDecodeMessageText(message, out var decodedText)) return;

//                parsedMessage = _feedMessageParser.ParseFeedMessageText(message, decodedText);

//                var messageDump = InputMessageDump.Create(parsedMessage.GetDumpMeta(), decodedText);
//                _inputDumper.Tell(messageDump, Self);

//                switch (parsedMessage)
//                {
//                    // Messages by owned product
//                    case FeedMessageParsed msg when _products.Contains(msg.Message.Product):
//                        Context.Parent.Tell(msg, Self);
//                        break;

//                    // BetSettlement is always accepted
//                    case FeedMessageParsed msg when msg.Message as BetSettlement != null:
//                        var newMsg = new FeedMessageParsed(msg.IncomingId, msg.ReceivedOn, (msg.Message as BetSettlement).CloneWithProduct(_products.First()));

//                        Context.Parent.Tell(newMsg , Self);
//                        break;

//                    // BetSettlementRollback is always accepted
//                    case FeedMessageParsed msg when msg.Message as BetSettlementRollback != null:
//                        var betSettlementRollbackMsg = new FeedMessageParsed(msg.IncomingId, msg.ReceivedOn, (msg.Message as BetSettlementRollback).CloneWithProduct(_products.First()));

//                        Context.Parent.Tell(betSettlementRollbackMsg, Self);
//                        break;

//                    case FeedMessageParsingFailed msg:
//                        _logger.Warn("FeedMessageParsingFailed MessageId={MessageId}, MessageType={MessageType}", message.IncomingId, msg.MessageType);

//                        _failureActor.Tell(msg, Self);
//                        break;
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.Error(e, "FeedMessageParsingFailed MessageId = {MessageId}", message.IncomingId);

//                _failureActor.Tell(new UnexpectedErrorOccured(
//                    failureTrigger: message,
//                    failureReason: e,
//                    lineService: (parsedMessage as FeedMessageParsed)?.Message.Product.ToLineService() ?? LineService.BetradarUnifiedFeed
//                ), Self);
//            }
//        }

//        protected bool TryDecodeMessageText(RawFeedMessage message, out string decodedText)
//        {
//            decodedText = null;
//            try
//            {
//                decodedText = Encoding.UTF8.GetString(message.Data.ToArray());
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.Warn("FeedMessageParsingFailed MessageId={MessageId}, ", message.IncomingId);

//                _failureActor.Tell(new FeedMessageDecodingFailed(
//                    incomingId: message.IncomingId,
//                    receivedOn: message.ReceivedOn,
//                    failureReason: ex,
//                    messageData: message.Data,
//                    messageType: FeedMessageType.Unknown
//                ), Self);
//                return false;
//            }
//        }

//        protected override void PostStop()
//        {
//            cancellationTokenSource.Cancel();

//            _channel?.Dispose();
//            _connection?.Dispose();
//        }
//    }
//}