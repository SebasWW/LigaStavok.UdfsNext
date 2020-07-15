using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Udfs.Transmitter;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TransmitterHostedService : IHostedService
	{
		private readonly TransmitterService transmitterService;

		public TransmitterHostedService()
		{

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

			var serviceBuilder = new TransmitterServiceBuilder(config);
				//.AddPlugin(new SportLevelPlugin(config, sportLevelPluginInjector));

			transmitterService = serviceBuilder.BuildService();

			try
			{

				transmitterService.Start();
			}
			catch (Exception ex)
			{

				throw;
			}

			
			var actor = transmitterService.GetUdfsActorSystem().ActorSelection(ActorPaths.Transmitter.Path);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			//transmitterService.Start();


			//var actor = transmitterService.GetUdfsActorSystem().ActorSelection(ActorPaths.Transmitter.Path);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			transmitterService.Stop();
			return Task.CompletedTask;
		}
	}
}
