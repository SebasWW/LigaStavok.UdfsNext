using Orleans;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public interface IProviderManagerGrain : IGrainWithIntegerKey
	{
		Task InitializeAsync();
	}
}