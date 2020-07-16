using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Concurrency;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	[Reentrant]
	public class ProviderManagerGrain : Grain, IProviderManagerGrain
	{
		private readonly ILogger<ProviderManagerGrain> logger;
		private readonly IProviderManager providerManager;
		private readonly ProviderManagerGrainOptions options;
		
		private CancellationTokenSource cancellationTokenSource;

		public ProviderManagerGrain(
			ILogger<ProviderManagerGrain> logger,
			IProviderManager providerManager,
			IOptions<ProviderManagerGrainOptions> options
		)
		{
			this.logger = logger;
			this.providerManager = providerManager;
			this.options = options.Value;
		}

		private async Task OnTimerTick(object obj)
		{
			try
			{
				await foreach(var messageContext in providerManager.WithCancellation(cancellationTokenSource.Token))
				{
					try
					{
						await GrainFactory.GetGrain<IFeedSubscriberGrain>(messageContext.Message.Id).InitializeAsync();
					}
					catch (Exception ex)
					{
						logger.LogError(ex, $"Calling ITranslationManagerGrain({messageContext.Message.Id}).");
						throw;
					}
				}
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				logger.LogError(ex, "EventSubscriberGrain initializing error.");
			}
		}

		public override Task OnActivateAsync()
		{
			cancellationTokenSource = new CancellationTokenSource();

			RegisterTimer(OnTimerTick, null, options.ActivatingProcessDelay, options.ActivatingProcessPeriod);

			var task = providerManager.StartAsync(CancellationToken.None);

			return task;
		}

		public override Task OnDeactivateAsync()
		{
			cancellationTokenSource.Cancel();
			return providerManager.StopAsync(CancellationToken.None);
		}

		public Task InitializeAsync()
		{
			return Task.CompletedTask;
		}

	}
}
