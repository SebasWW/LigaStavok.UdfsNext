using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public interface IFeedListener
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}