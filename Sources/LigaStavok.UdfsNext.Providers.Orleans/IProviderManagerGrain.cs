using Orleans;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Providers.Orleans
{
	public interface IProviderManagerGrain : IGrainWithIntegerKey
	{
		Task InitializeAsync();
	}
}