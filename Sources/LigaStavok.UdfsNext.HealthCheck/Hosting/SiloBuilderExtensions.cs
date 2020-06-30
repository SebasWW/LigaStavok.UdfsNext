using LigaStavok.UdfsNext.HealthCheck.Orleans;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.HealthCheck.Hosting
{
	public static class SiloBuilderExtensions
	{
		public static ISiloBuilder UseUdfsClusterHealthChecks(this ISiloBuilder siloBuilder)
		{
            siloBuilder.ConfigureServices(services =>
            {
                services.AddHealthChecks()
                    .AddCheck<GrainHealthCheck>("GrainHealth")
                    .AddCheck<SiloHealthCheck>("SiloHealth")
                    .AddCheck<StorageHealthCheck>("StorageHealth")
                    .AddCheck<ClusterHealthCheck>("ClusterHealth");

                services.AddSingleton<IHealthCheckPublisher, LoggingHealthCheckPublisher>()
                    .Configure<HealthCheckPublisherOptions>(options =>
                    {
                        options.Period = TimeSpan.FromSeconds(1);
                    });

                //services.AddSingleton(client);
                //services.AddSingleton(Enumerable.AsEnumerable(new IHealthCheckParticipant[] { oracle }));
            });

            return siloBuilder;
		}
	}
}
