using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.BetRadar.State;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Orleans
{
	public class FeedSubscriberGrainState
	{
		public TranslationState Translation { get; set; }
	}
}