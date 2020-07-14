//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using NLog;
//using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters;
//using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Configuration;
//using Udfs.Transmitter.Messages.Interfaces;
//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
//using Microsoft.Extensions.Logging;
//using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

//namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
//{
//    public sealed class TransmitterCommandsFactory 
//    {
//        private readonly AdapterConfiguration adapterConfiguration;
//		private readonly Logger<TransmitterCommandsFactory> logger;
//		private readonly IDataEventAdapter dataEventAdapter;
//		private readonly IMarketEventAdapter marketEventAdapter;
//		private readonly ITranslationAdapter translationAdapter;
//		private readonly IBetStartEventAdapter betStartEventAdapter;
//		private readonly IBetStopEventAdapter betStopEventAdapter;
//		private readonly IPingMessageAdapter pingMessageAdapter;

//		public TransmitterCommandsFactory(
//            AdapterConfiguration adapterConfiguration,
//            Logger<TransmitterCommandsFactory> logger,

//            IDataEventAdapter dataEventAdapter,
//            IMarketEventAdapter marketEventAdapter,
//            ITranslationAdapter translationAdapter,
//            IBetStartEventAdapter betStartEventAdapter,
//            IBetStopEventAdapter betStopEventAdapter,
//            IPingMessageAdapter pingMessageAdapter
//        )
//        {
//            this.adapterConfiguration = adapterConfiguration;
//			this.logger = logger;
//			this.dataEventAdapter = dataEventAdapter;
//			this.marketEventAdapter = marketEventAdapter;
//			this.translationAdapter = translationAdapter;
//			this.betStartEventAdapter = betStartEventAdapter;
//			this.betStopEventAdapter = betStopEventAdapter;
//			this.pingMessageAdapter = pingMessageAdapter;
//		}

//		public Task<IEnumerable<ITransmitterCommand>> SendPingMessageAsync(MessageContext<PingMessage> messageContext)
//		{
//			return Task.FromResult(pingMessageAdapter.Adapt(messageContext));
//		}

//        public Task<IEnumerable<ITransmitterCommand>> SendTranslationAsync(MessageContext<Translation> messageContext)
//        {
//            var (Ok, Reason) = ClientActor.CheckTranslation(messageContext.Message, adapterConfiguration);
//            if (!Ok)
//            {
//                logger.LogWarning($"Translation validation is failed. Id: {messageContext.Message.Id}, Reason: {Reason}, MessageId: {messageContext.IncomingId}");
//                return Array.Empty<ITransmitterCommand>();
//            }

//            try
//            {
//                var commands = translationAdapter.Adapt(messageContext);
//                return Task.FromResult(commands);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, $"Translation transmitter commands generation error. TranslationId: {messageContext.Message.Id}, MessageId: {messageContext.IncomingId}");
//                return Array.Empty<ITransmitterCommand>();
//            }
//        }

//        public async Task<IEnumerable<ITransmitterCommand>> SendEventDataAsync(MessageContext<EventData> messageContext)
//        {
//            switch (messageContext.Message.EventCode)
//            {
//                case EventCode.HEARTBEAT:
//                    return Array.Empty<ITransmitterCommand>();

//                case EventCode.MARKET:
//                    return await marketEventAdapter.AdaptAsync(messageContext);

//                case EventCode.BETSTART:
//                    return betStartEventAdapter.Adapt(messageContext);

//                case EventCode.BETSTOP:
//                    return betStopEventAdapter.Adapt(messageContext);

//                //case EventCode.GAME_START_DELAYED:
//                //case EventCode.WARM_UP:
//                //case EventCode.PLAYERS_ON_PITCH:
//                //case EventCode.MEDICAL_TIMEOUT:
//                //case EventCode.TIMEOUT:
//                //case EventCode.PLAYER_WITHDRAWAL:
//                //case EventCode.GAME_INTERRUPTED:
//                //case EventCode.FINISH_GAME:
//                //case EventCode.START_PERIOD:
//                //case EventCode.POSESSION_1:
//                //case EventCode.POSESSION_2:

//                default:
//                    return await dataEventAdapter.AdaptAsync(messageContext);
//            }
//        }
//    }
//}
