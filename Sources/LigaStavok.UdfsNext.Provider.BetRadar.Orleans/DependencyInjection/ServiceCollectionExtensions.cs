using LigaStavok.UdfsNext.Providers;
using LigaStavok.UdfsNext.Providers.Orleans;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetRadarProviderOrleans(
            this IServiceCollection services
        ) 
        {
            services.AddSingleton<ITranslationDistributer, TranslationDistributer>();

            return services;
        }
    }
}
