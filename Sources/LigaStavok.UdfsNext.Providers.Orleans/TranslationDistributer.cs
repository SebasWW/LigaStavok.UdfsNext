using System.Threading.Tasks;
using Orleans;

namespace LigaStavok.UdfsNext.Providers.Orleans
{
	public class TranslationDistributer : ITranslationDistributer
	{
		private readonly IGrainFactory grainFactory;

		public TranslationDistributer(IGrainFactory grainFactory)
		{
			this.grainFactory = grainFactory;
		}

		public Task Distribute(long translationId)
		{
			return grainFactory.GetGrain<IFeedSubscriberGrain>(translationId).InitializeAsync();
		}
	}
}
