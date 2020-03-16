using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.HealthCheck.Hosting
{
	public static class HostBuilderExtensions
	{
		public static IHostBuilder UseUdfsHealthChecks(this IHostBuilder hostBuilder, Action<IHealthChecksBuilder> builderDelegate)
        {
            //.ConfigureLogging(builder =>
            //{
            //    builder.AddConsole();
            //})
            //.Configure(app =>
            //{
            //    app.UseHealthChecks(myOptions.Value.PathString);
            //})
            return hostBuilder;
        }
	} 
}

