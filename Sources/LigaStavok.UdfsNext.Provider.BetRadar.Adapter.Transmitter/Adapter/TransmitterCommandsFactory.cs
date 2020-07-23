using System.Collections.Generic;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Configuration;
using Udfs.Transmitter.Messages.Interfaces;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter
{
    public sealed class TransmitterCommandsFactory : ITransmitterCommandsFactory
    {
        private readonly AdapterConfiguration _adapterConfiguration;
		private readonly IAliveAdapter aliveAdapter;
		private readonly IBetCancelAdapter betCancelAdapter;
		private readonly IBetCancelRollbackAdapter betCancelRollbackAdapter;
		private readonly IBetSettlementAdapter betSettlementAdapter;
		private readonly IBetSettlementRollbackAdapter betSettlementRollbackAdapter;
		private readonly IBetStopAdapter betStopAdapter;
		private readonly IOddsChangeAdapter oddsChangeAdapter;
		private readonly IFixtureAdapter fixtureAdapter;
		private readonly IFixtureListAdapter fixtureListAdapter;
		private readonly IMarketDescriptionListAdapter marketDescriptionListAdapter;
		private readonly IMatchSummaryAdapter matchSummaryAdapter;
		private readonly IScheduleAdapter scheduleAdapter;

		public TransmitterCommandsFactory(
            AdapterConfiguration adapterConfiguration,

            IAliveAdapter aliveAdapter,
            IBetCancelAdapter betCancelAdapter,
            IBetCancelRollbackAdapter betCancelRollbackAdapter,
            IBetSettlementAdapter betSettlementAdapter,
            IBetSettlementRollbackAdapter betSettlementRollbackAdapter,
            IBetStopAdapter betStopAdapter,
            IOddsChangeAdapter oddsChangeAdapter,

            IFixtureAdapter fixtureAdapter,
            IFixtureListAdapter fixtureListAdapter,
            IMarketDescriptionListAdapter marketDescriptionListAdapter,
            IMatchSummaryAdapter matchSummaryAdapter,
            IScheduleAdapter scheduleAdapter
        )
        {
            _adapterConfiguration = adapterConfiguration;
			this.aliveAdapter = aliveAdapter;
			this.betCancelAdapter = betCancelAdapter;
			this.betCancelRollbackAdapter = betCancelRollbackAdapter;
			this.betSettlementAdapter = betSettlementAdapter;
			this.betSettlementRollbackAdapter = betSettlementRollbackAdapter;
			this.betStopAdapter = betStopAdapter;
			this.oddsChangeAdapter = oddsChangeAdapter;
			this.fixtureAdapter = fixtureAdapter;
			this.fixtureListAdapter = fixtureListAdapter;
			this.marketDescriptionListAdapter = marketDescriptionListAdapter;
			this.matchSummaryAdapter = matchSummaryAdapter;
			this.scheduleAdapter = scheduleAdapter;
		}

        public IEnumerable<ITransmitterCommand> CreateTransmitterCommands(MessageContext<AdaptMessage> messageContext)
        {
            switch (messageContext.Message.Message)
            {
                // Feed
                case Alive alive:
                    return aliveAdapter.Adapt(messageContext.Next(alive));
                case BetCancel betCancel:
                    return betCancelAdapter.Adapt(messageContext.Next(betCancel));
                case BetCancelRollback betCancelRollback:
                    return betCancelRollbackAdapter.Adapt(messageContext.Next(betCancelRollback));
                case BetSettlement betSettlement:
                    return betSettlementAdapter.Adapt(messageContext.Next(betSettlement));
                case BetSettlementRollback betSettlementRollback:
                    return betSettlementRollbackAdapter.Adapt(messageContext.Next(betSettlementRollback));
                case BetStop betStop:
                    return betStopAdapter.Adapt(messageContext.Next(betStop));
                case OddsChange oddsChange:
                    return oddsChangeAdapter.Adapt(messageContext.Next(oddsChange), messageContext.Message.LineService, _adapterConfiguration.Messages.OddsChange);
                //case FixtureChange fixtureChange:
                //        return new ITransmitterCommand[0];

                // Api
                case Fixture fixture:
                    return fixtureAdapter.Adapt(messageContext.Next(fixture), messageContext.Message.Language, messageContext.Message.LineService);
                case FixtureList fixtureList:
                    return fixtureListAdapter.Adapt(messageContext.Next(fixtureList), messageContext.Message.Language, messageContext.Message.LineService);
                case MarketDescriptionList marketDescriptionList:
                    return marketDescriptionListAdapter.Adapt(messageContext.Next(marketDescriptionList), messageContext.Message.Language, messageContext.Message.LineService);
                case MatchSummary matchSummary:
                    return matchSummaryAdapter.Adapt(messageContext.Next(matchSummary), messageContext.Message.Language, messageContext.Message.LineService);
                case Schedule schedule:
                    return scheduleAdapter.Adapt(messageContext.Next(schedule), messageContext.Message.Language, messageContext.Message.LineService);
                default:
                    return Array.Empty<ITransmitterCommand>();
            }
        }
    }
}