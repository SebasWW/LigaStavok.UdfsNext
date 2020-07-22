using System;
using LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBetRadarRunTime(
            this IServiceCollection services, 
            Action<IProviderBuilder> configureDelegate
        ) 
        {
            IProviderBuilder builder = new ProviderBuilder(services);
            configureDelegate.Invoke(builder);

            return services;
        }
    }
}
