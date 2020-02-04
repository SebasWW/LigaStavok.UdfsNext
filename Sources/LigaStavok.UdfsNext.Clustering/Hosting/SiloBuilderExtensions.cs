using System;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Clustering.Hosting
{
    public static class SiloBuilderExtensions
	{
        public static void ConfigureUdfs(this ISiloBuilder siloBuilder, UdfsClusterOptions udfsClusterOptions)
		{
            // Local or distribute cluster
            if (udfsClusterOptions.Membership.Enabled)
                siloBuilder
                    .UseAdoNetClustering(options =>
                    {
                        options.Invariant = udfsClusterOptions.Membership.Provider;
                        options.ConnectionString = udfsClusterOptions.Membership.ConnectionString;
                    })
                    .Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = udfsClusterOptions.EndPoint.AdvertisedIPAddress ?? IPAddress.Loopback;
                        options.GatewayListeningEndpoint = udfsClusterOptions.EndPoint.GatewayListeningEndpoint;
                        options.GatewayPort = udfsClusterOptions.EndPoint.GatewayPort;
                        options.SiloListeningEndpoint = udfsClusterOptions.EndPoint.SiloListeningEndpoint;
                        options.SiloPort = udfsClusterOptions.EndPoint.SiloPort;
                    });
            else
                siloBuilder.UseLocalhostClustering();

            // Cluster identity
            siloBuilder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = udfsClusterOptions.ClusterService.ClusterId;
                    options.ServiceId = udfsClusterOptions.ClusterService.ServiceId;
                });

            // Interfaces
            foreach (var item in udfsClusterOptions.GrainAssemblies)
                siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(item).WithReferences());

           // siloBuilder
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    //TODO: Logging slows down testing a bit.
                //    //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddConsole();
                //              logging.AddDebug();
                //          })
                //.UsePerfCounterEnvironmentStatistics()
                //.Configure<SiloMessagingOptions>(options =>
                //{
                //    options.ResponseTimeout = TimeSpan.FromSeconds(5);
                //})
                //.Configure<GrainCollectionOptions>(options =>
                //{
                //    // Set the value of CollectionAge to 10 minutes for all grain
                //    options.CollectionAge = TimeSpan.FromMinutes(10);

                //    // Override the value of CollectionAge to 5 minutes for MyGrainImplementation
                //    // options.ClassSpecificCollectionAge[typeof(AdoNetClusteringSiloOptions).FullName] = TimeSpan.FromMinutes(5);
                //});

            // Reminder
            if (udfsClusterOptions.Reminder.Enabled)
                siloBuilder
                    .UseAdoNetReminderService(options =>
                    {
                        options.Invariant = udfsClusterOptions.Reminder.Provider;
                        options.ConnectionString = udfsClusterOptions.Reminder.ConnectionString;
                    });

            // Storage
            if (udfsClusterOptions.Storage.Enabled)
                siloBuilder
                    .AddAdoNetGrainStorage("UdfsGrainStorage", options =>
                    {
                        options.Invariant = udfsClusterOptions.Storage.Provider;
                        options.ConnectionString = udfsClusterOptions.Storage.ConnectionString;
                    });

            //if (context.HostingEnvironment.IsDevelopment())
            //    builder.UseDevelopmentClustering(new IPEndPoint(6, 11111));
        }
    }
}
