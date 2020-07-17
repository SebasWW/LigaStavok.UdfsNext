using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Udfs.Transmitter;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
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
