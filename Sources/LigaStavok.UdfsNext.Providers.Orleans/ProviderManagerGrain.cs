using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Concurrency;

namespace LigaStavok.UdfsNext.Providers.Orleans
{
	[Reentrant]
	public class ProviderManagerGrain : Grain, IProviderManagerGrain
	{
		private readonly IProviderManager providerManager;

		public ProviderManagerGrain(
			IProviderManager providerManager
		)
		{
			this.providerManager = providerManager;
		}

		public override Task OnActivateAsync()
		{
			return providerManager.StartAsync(CancellationToken.None);
		}

		public override Task OnDeactivateAsync()
		{
			return providerManager.StopAsync(CancellationToken.None);
		}

		public Task InitializeAsync()
		{
			return Task.CompletedTask;
		}

	}
}
