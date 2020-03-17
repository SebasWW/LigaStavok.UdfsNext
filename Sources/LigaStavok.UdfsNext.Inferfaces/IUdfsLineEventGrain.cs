using System.Threading.Tasks;
using LigaStavok.UdfsNext.Remoting;
using Orleans;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineEventGrain : IGrainWithIntegerKey
	{
		Task<UdfsResponse<LineEvent>> GetAsync(UdfsRequest request);
	}
}
