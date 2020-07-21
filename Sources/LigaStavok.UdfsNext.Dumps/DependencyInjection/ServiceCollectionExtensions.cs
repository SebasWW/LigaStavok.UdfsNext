using System;
using LigaStavok.UdfsNext.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageDumping(
            this IServiceCollection services, 
            Action<MessageDumperBuilder> configureHandler
        ) 
        {
            var builder = new MessageDumperBuilder(services);
            configureHandler.Invoke(builder);
            builder.Build();
            
            return services;
        }
    }
}
