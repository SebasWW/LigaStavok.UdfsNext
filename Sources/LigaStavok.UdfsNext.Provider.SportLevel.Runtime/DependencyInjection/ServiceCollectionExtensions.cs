using System;
using LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelRunTime(
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
