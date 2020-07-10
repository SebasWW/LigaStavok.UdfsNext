using System;
using LigaStavok.Processors.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProcessors(
            this IServiceCollection services, 
            Action<ProcessorsServiceBuilder> configureDelegate
        ) 
        {
            var builder = new ProcessorsServiceBuilder(services);
            configureDelegate.Invoke(builder);

            services.AddSingleton(builder);
            services.AddHostedService<ProcessorsService>();
            return services;
        }
    }
}
