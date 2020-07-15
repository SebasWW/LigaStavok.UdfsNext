using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow;
using System;
using System.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelTransmitterAdapter(
            this IServiceCollection services
        ) 
        {
            // Adapters
            services.AddSingleton<IBetStartEventAdapter, BetStartEventAdapter>();
            services.AddSingleton<IBetStopEventAdapter, BetStopEventAdapter>();
            services.AddSingleton<IDataEventAdapter, DataEventAdapter>();
            services.AddSingleton<IMarketEventAdapter, MarketEventAdapter>();
            services.AddSingleton<IPingMessageAdapter, PingMessageAdapter>();
            services.AddSingleton<ITranslationAdapter,TranslationAdapter>();

            // Runtime
            services.AddSingleton<ITransmitterCommandsFactory, TransmitterCommandsFactory>();
            services.AddSingleton<AdapterDataFlow>();
            services.AddSingleton<IProviderAdapter, TransmitterAdapter>();
            services.AddSingleton<ITransmitterHost, TransmitterAdapterHost>();

            // Service
            services.AddHostedService<TransmitterAdapterHostService>();

            return services;
        }
    }
}
