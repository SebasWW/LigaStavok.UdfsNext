using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class ProviderManagerStartupTask : IStartupTask
    {
        private readonly IGrainFactory grainFactory;

        public ProviderManagerStartupTask(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var grain = this.grainFactory.GetGrain<IProviderManagerGrain>(0);
            await grain.InitializeAsync();
        }
    }
}
