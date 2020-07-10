using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface IProviderManager : IDisposable, IAsyncEnumerable<MessageContext<Translation>>
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}