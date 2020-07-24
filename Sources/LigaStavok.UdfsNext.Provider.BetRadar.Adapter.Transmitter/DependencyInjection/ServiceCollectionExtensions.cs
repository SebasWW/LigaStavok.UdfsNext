using LigaStavok.UdfsNext.Provider.Adapter;
using LigaStavok.UdfsNext.Provider.BetRadar;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.DataFlow;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetRadarTransmitterAdapter(
            this IServiceCollection services
        ) 
        {
            // Adapters
            services.AddSingleton<IAliveAdapter, AliveAdapter>();
            services.AddSingleton<IBetCancelAdapter, BetCancelAdapter>();
            services.AddSingleton<IBetCancelRollbackAdapter, BetCancelRollbackAdapter>();
            services.AddSingleton<IBetSettlementAdapter, BetSettlementAdapter>();
            services.AddSingleton<IBetSettlementRollbackAdapter, BetSettlementRollbackAdapter>();
            services.AddSingleton<IBetStopAdapter, BetStopAdapter>();
            services.AddSingleton<IOddsChangeAdapter, OddsChangeAdapter>();

            services.AddSingleton<IFixtureAdapter, FixtureAdapter>();
            services.AddSingleton<IFixtureListAdapter, FixtureListAdapter>();
            services.AddSingleton<IMarketDescriptionListAdapter, MarketDescriptionListAdapter>();
            services.AddSingleton<IMatchSummaryAdapter, MatchSummaryAdapter>();
            services.AddSingleton<IScheduleAdapter, ScheduleAdapter>();

            // Runtime
            services.AddSingleton<ITransmitterCommandsFactory, TransmitterCommandsFactory>();
            services.AddSingleton<AdapterDataFlow>();
            services.AddSingleton<IProviderAdapter, TransmitterAdapter>();
            services.AddSingleton<ITransmitterAdapterHost, TransmitterAdapterHost>();

            // Service
            services.AddHostedService<TransmitterAdapterHostService>();

            return services;
        }
    }
}
