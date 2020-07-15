using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Udfs.Common.Messages;
using Udfs.Transmitter;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Interfaces;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TransmitterAdapterHost : ITransmitterHost
	{
		private readonly TransmitterService transmitterService;

		public TransmitterAdapterHost()
		{

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

			var serviceBuilder = new TransmitterServiceBuilder(config);
				//.AddPlugin(new SportLevelPlugin(config));

			transmitterService = serviceBuilder.BuildService();
		}

		public void SendCommand(ITransmitterCommand transmitterCommand)
		{
			transmitterService.SendCommand(transmitterCommand);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			transmitterService.Start();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			transmitterService.Stop();
			return Task.CompletedTask;
		}
	}
}
