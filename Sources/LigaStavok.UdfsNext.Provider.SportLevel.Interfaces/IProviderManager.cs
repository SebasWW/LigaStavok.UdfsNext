using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface IProviderManager : IDisposable
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);

		Task<IDictionary<long, TranslationSubscription>> GetSubscriptionDetailsAsync();

	}
}