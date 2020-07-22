using System.Threading;
using System.Threading.Tasks;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.Adapter
{
	public interface ITransmitterAdapterHost
	{
		void SendCommand(ITransmitterCommand transmitterCommand);
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}