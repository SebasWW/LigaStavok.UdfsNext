using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LigaStavok.UdfsNext.Line.Grains;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Line.Test.Server
{
	class Program
	{
		public static Task Main(string[] args)
		{
            return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "HelloWorldApp";
                        })
                        //.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UdfsLineEventGrain).Assembly).WithReferences())
                      //  .AddMemoryGrainStorage(name: "ArchiveStorage");
                    //.AddAzureBlobGrainStorage(
                    //    name: "profileStore",
                    //    configureOptions: options =>
                    //    {
                    //        // Use JSON for serializing the state in storage
                    //        options.UseJson = true;

                    //        // Configure the storage connection key
                    //        options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=data1;AccountKey=SOMETHING1";
                    //    })
                    ;
                })
                .ConfigureServices(services =>
                {
                    services.Configure<ConsoleLifetimeOptions>(options =>
                    {
                        options.SuppressStatusMessages = true;
                    });
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .RunConsoleAsync();
        }
	}
}
