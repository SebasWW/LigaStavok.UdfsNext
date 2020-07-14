//using Akka.Actor;
//using Akka.DI.Core;
//using Akka.Routing;
//using Newtonsoft.Json;
//using NLog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Udfs.Common.Actor;
//using Udfs.Common.Messages;
//using Udfs.SportLevel.Plugin.Abstractions;
//using Udfs.SportLevel.Plugin.Api.Responses;
//using Udfs.SportLevel.Plugin.Dumping;
//using Udfs.SportLevel.Plugin.Dumping.Messages;
//using Udfs.SportLevel.Plugin.Failure;
//using Udfs.SportLevel.Plugin.Failure.Messages;
//using Udfs.SportLevel.Plugin.Feed.Messages;
//using Udfs.SportLevel.Plugin.State;
//using Udfs.SportLevel.Plugin.Tools;
//using Udfs.Transmitter.Messages.Interfaces;
//using Udfs.Transmitter.Plugin;

//namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
//{
//    public sealed class AdapterActor : SportLevelActorBase
//    {
//        private readonly DumpingOptions _dumpingOptions;
//        private readonly IStateManager stateManager;
//        private readonly ITransmitterCommandsFactory _commandsFactory;
//        private readonly ICanTell _transmitterActor;
//        private readonly ICanTell _stateActor;
//        private readonly ICanTell _failureActor;
//        private readonly ICanTell _dumperActor;
//        private readonly ILogger _logger;

//        public AdapterActor(
//            IStateManager stateManager, 
//            ITransmitterCommandsFactory commandsFactory,
//            ActorMetadata<TransmitterRootActor> transmitterActorMeta,
//            ActorMetadata<FailureActor> failureActorMeta,
//            ActorMetadata<DumpActor> dumperActorMetadata,
//            ActorMetadata<StateActor> stateActorMeta,
//            DumpingOptions dumpingOptions,
//            LogFactory logFactory
//        ) : base(logFactory)
//        {
//            _logger = logFactory.GetCurrentClassLogger();

//            _dumpingOptions = dumpingOptions;
//            this.stateManager = stateManager;
//            _commandsFactory = commandsFactory;


//            //var apiRouter = new SmallestMailboxPool(1);
//            //var apiProps = Context.DI().Props(stateActorMeta.ActorType).WithRouter(apiRouter);
//            //_stateActor =  Context.ActorOf(apiProps, stateActorMeta.Name); 
//            _stateActor = Context.ActorSelection(stateActorMeta.Path);
//            _transmitterActor = Context.ActorSelection(transmitterActorMeta.Path);
//            _failureActor = Context.ActorSelection(failureActorMeta.Path);
//            _dumperActor = Context.ActorSelection(dumperActorMetadata);

//            Receive<ShutdownCommand>(msg => OnShutdownCommand(msg));

//            // api
//            Receive<MessageContext<Translation>>(async context => await ProcessMessage(context));

//            // feed
//            Receive<MessageContext<PingMessage>>(async context => await ProcessMessage(context));
//            Receive<MessageContext<DataEvent>>(async context => await ProcessMessage(context));
            
//        }

//        private void OnShutdownCommand(ShutdownCommand _)
//        {
//            Context.Stop(Self);
//        }

//        private async Task ProcessMessage(IMessageContext context)
//        {
//            try
//            {
//                var transmitterCommands = await CreateTransmitterCommandsAsync(context);

//                foreach (var command in transmitterCommands)
//                {
//                    if (_dumpingOptions.UseToTransmitterDumps)
//                        DumpCommand(context.Next(command));
//                    if (_dumpingOptions.UseTransmitter)
//                        _transmitterActor.Tell(command, Self);
//                }

//                if (context is MessageContext<DataEvent> c)
//                {
//                    var id = long.Parse(c.Message.TranslationId);
                    
//                    var state = await stateManager.GetTranslationStateAsync(id);
//                    state.LastMessageId = c.Message.EventNumber;

//                    var msg = new SaveTranslationStateMessage(id);
//                    _stateActor.Tell(context.Next(msg), Self);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.Error(ex, "AdaptMessage IncomingId={IncomingId} ReceivedOn={ReceivedOn} Message={Message}",
//                    context.IncomingId, context.ReceivedOn, context.GetMessage());

//                _failureActor.Tell(context.Next(new UnexpectedErrorOccured(ex)), Self);
//            }
//        }

//        private async Task<IEnumerable<ITransmitterCommand>> CreateTransmitterCommandsAsync(IMessageContext context)
//        {
//            var transmitterCommands = Enumerable.Empty<ITransmitterCommand>();
//            try
//            {
//                transmitterCommands = await _commandsFactory.CreateTransmitterCommandsAsync(context, stateManager, _logger);
//            }
//            catch (Exception ex)
//            {
//                _logger.Error(ex, "AdaptMessage IncomingId={IncomingId} ReceivedOn={ReceivedOn} Message={Message}",
//                    context.IncomingId, context.ReceivedOn, context.GetMessage());

//                var errorMessage = new MessageAdaptionFailed
//                (
//                    failureReason: ex,
//                    messageType: context.GetMessage().GetType()
//                );

//                _failureActor.Tell(context.Next(errorMessage), Self);
//            }

//            return transmitterCommands;
//        }

//        private void DumpCommand(MessageContext<ITransmitterCommand> context)
//        {

//            var eventId = context.Message is IGameEventRelatedCommand command
//                ? command.GameEventId.GetFileNameSafeEventId()
//                : "Line";

//            var messageDump = MessageDump.Create(
//                new DumpMeta(
//                    DumpSource.ToTransmitter,
//                    context.Message.GetType().Name,
//                    fixtureId: eventId
//                ),
//                JsonConvert.SerializeObject(context.Message)
//            );

//            _dumperActor.Tell(context.Next(messageDump), Self);
//        }
//    }
//}