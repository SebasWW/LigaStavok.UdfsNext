using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public class TransmitterAdapterHostService : IHostedService
	{
		private readonly ITransmitterHost transmitterHost;

		public TransmitterAdapterHostService(
			ITransmitterHost transmitterHost	
		)
		{
			this.transmitterHost = transmitterHost;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			transmitterHost.StartAsync(cancellationToken);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			transmitterHost.StopAsync(cancellationToken);
			return Task.CompletedTask;
		}
	}
}
