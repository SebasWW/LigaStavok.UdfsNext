using System;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Dumps.DependencyInjection;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans;
using LigaStavok.UdfsNext.Provider.SportLevel.Orleans.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportLevelProviderOrleans(
            this IServiceCollection services, 
            ServiceConfiguration configuration
        ) 
        {
            // Logging
            services
                //.Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"))
                .AddLogging(
                    configure =>
                    {
                        configure.AddProvider(new NLogLoggerProvider());
                    }
                )

            .Configure<ProviderManagerGrainOptions>(
                options =>
                {
                }
            )

            // Dumps
            .AddMessageDumping(
                builder =>
				{
                    if (configuration.Dump.SqlServer.Enabled)
                        builder.ConfigureSqlServerDumping(
                            options =>
                            {
                                options.Configure(configuration.Dump.SqlServer);
                            }
                       );

                    if (configuration.Dump.FileSystem.Enabled)
                        builder.ConfigureFileSystemDumping(
                            options =>
                            {
                                options.Configure(configuration.Dump.FileSystem);
                            }
                       );
                }   
             )

            // Adapter
            .AddSportLevelTransmitterAdapter()
            .AddSingleton(configuration.Adapter) // Перепилить на Options

            // Runtime
            .AddSportLevelRunTime(
                builder =>
				{
                    builder.ConfigureProviderManager(
                        options =>
                        {
                            options.MetaRefreshInterval = configuration.Provider.MetaRefreshInterval;
                        }
                    );

                    builder.ConfigureFeedListener(
                        options =>
                        {
                            options.UserName = configuration.Provider.UserName;
                            options.Password = configuration.Provider.Password;
                        }
                    );

                    builder.ConfigureHttpClientManager(
                        options =>
                        {
                            options.UserName = configuration.Provider.UserName;
                            options.Password = configuration.Provider.Password;
                            options.Uri = new Uri(configuration.Provider.WebApiUrl);
                        }
                    );

                    builder.AddWebSocketClient(
                        options =>
                        {
                            options.UseDefaultCredentials = true;
                            options.Uri = new Uri(configuration.Provider.WebSocketUrl);
                        }
                    );
                }
            );

            return services;
        }
    }
}
