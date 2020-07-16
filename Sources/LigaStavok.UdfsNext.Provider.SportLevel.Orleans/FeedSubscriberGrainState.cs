using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.State;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class FeedSubscriberGrainState
	{
		public TranslationState Translation { get; set; }
	}
}