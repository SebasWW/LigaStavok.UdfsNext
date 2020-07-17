using System;
using System.Net;
using LigaStavok.UdfsNext.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LigaStavok.UdfsNext.Hosting
{
	public static class SiloBuilderExtensions
	{
        public static void Configure(this ISiloBuilder siloBuilder, OrleansClusterConfiguration configuration)
		{
            // Local or distribute cluster
            if (configuration.Membership.Enabled)
                siloBuilder
                    .UseAdoNetClustering(options =>
                    {
                        options.Invariant = configuration.Membership.Provider;
                        options.ConnectionString = configuration.Membership.ConnectionString;
                    })
                    .Configure<EndpointOptions>(options =>
                    {

                        options.SiloPort = configuration.Endpoint.SiloPort;
                        options.GatewayPort = configuration.Endpoint.GatewayPort;

                        // IP Address to advertise in the cluster
                        if (IPAddress.TryParse(configuration.Endpoint.AdvertisedIPAddress, out var iPAddress)) options.AdvertisedIPAddress = iPAddress;

                        // The socket used for silo-to-silo will bind to this endpoint
                        if (configuration.Endpoint.GatewayListeningPort > 0 && IPAddress.TryParse(configuration.Endpoint.AdvertisedIPAddress, out var gatewayIPAddress))
                            options.GatewayListeningEndpoint = new IPEndPoint(gatewayIPAddress, configuration.Endpoint.GatewayListeningPort);

                        // The socket used by the gateway will bind to this endpoint
                        if (configuration.Endpoint.SiloListeningPort > 0 && IPAddress.TryParse(configuration.Endpoint.SiloListeningIP, out var siloIPAddress))
                            options.SiloListeningEndpoint = new IPEndPoint(siloIPAddress, configuration.Endpoint.SiloListeningPort);
                    });
            else
                siloBuilder.UseLocalhostClustering();

            // Cluster identity
            siloBuilder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = configuration.ClusterId;
                    options.ServiceId = configuration.ServiceId;
                })

                .Configure<GrainCollectionOptions>(options =>
                {
                    // Set the value of CollectionAge to 3 minutes for all grain
                    options.CollectionAge = TimeSpan.FromMinutes(3);

                    // Override the value of CollectionAge to 5 minutes for MyGrainImplementation
                    // options.ClassSpecificCollectionAge[typeof(FeedSubscriberGrain).FullName] = TimeSpan.FromMinutes(5);
                });

            //.UsePerfCounterEnvironmentStatistics()
            //.Configure<SiloMessagingOptions>(options =>
            //{
            //    options.ResponseTimeout = TimeSpan.FromSeconds(5);
            //})

            // Reminder
            if (configuration.Reminder.Enabled)
                siloBuilder
                    .UseAdoNetReminderService(options =>
                    {
                        options.Invariant = configuration.Reminder.Provider;
                        options.ConnectionString = configuration.Reminder.ConnectionString;
                    });
            else
                siloBuilder
                    .UseInMemoryReminderService();

            // Storage
            if (configuration.Storage.Enabled)
                siloBuilder
                    .AddAdoNetGrainStorage("stateStore", options =>
                    {
                        options.Invariant = configuration.Storage.Provider;
                        options.ConnectionString = configuration.Storage.ConnectionString;
                    });
            else
                siloBuilder.AddMemoryGrainStorage("stateStore");

            //if (context.HostingEnvironment.IsDevelopment())
            //    builder.UseDevelopmentOrleans(new IPEndPoint(6, 11111));


            // HealthChecks
            //siloBuilder.UseUdfsClusterHealthChecks();
        }
    }
}
