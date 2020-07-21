using System;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;
using LigaStavok.UdfsNext.Provider.SportLevel;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelProviderOrleans(
            this IServiceCollection services,
            Action<ProviderManagerGrainOptions> optionHandler
        ) 
        {
            services
                .Configure(optionHandler);

            services.AddSingleton<ITranslationDistributer, TranslationDistributer>();

            return services;
        }
    }
}
