using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans.StartupTasks
{
	public class ProviderManagerStartupTask : IStartupTask
    {
        private readonly IGrainFactory grainFactory;
		private readonly ILogger<ProviderManagerStartupTask> logger;
		private CancellationToken cancellationToken;

		public ProviderManagerStartupTask(IGrainFactory grainFactory, ILogger<ProviderManagerStartupTask> logger)
        {
            this.grainFactory = grainFactory;
			this.logger = logger;
		}

        public Task Execute(CancellationToken cancellationToken)
		{
			this.cancellationToken = cancellationToken;
			var task = Task.Run(Execute);

			return Task.CompletedTask;
		}

		public async Task Execute()
		{
			while (!cancellationToken.IsCancellationRequested)
			{
                var grain = this.grainFactory.GetGrain<IProviderManagerGrain>(0);

				try
				{
                    await grain.InitializeAsync();
                }
				catch (Exception ex)
				{
					logger.LogError(ex, "ProviderManagerGrain calling error.");
				}

                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
            }
        }
    }
}
