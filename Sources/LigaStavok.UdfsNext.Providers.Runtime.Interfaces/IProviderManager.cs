using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Providers
{
	public interface IProviderManager : IDisposable
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}