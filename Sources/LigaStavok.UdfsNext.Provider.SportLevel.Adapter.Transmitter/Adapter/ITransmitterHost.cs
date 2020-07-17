using System.Threading;
using System.Threading.Tasks;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public interface ITransmitterHost
	{
		void SendCommand(ITransmitterCommand transmitterCommand);
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}