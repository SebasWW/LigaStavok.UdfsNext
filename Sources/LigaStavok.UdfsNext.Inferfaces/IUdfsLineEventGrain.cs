using System.Threading.Tasks;
using LigaStavok.UdfsNext.Abstractions;
using LigaStavok.UdfsNext.Line;
using Orleans;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineEventGrain : IGrainWithIntegerKey
	{
		Task<int> GetLineEvent(int request);
	}
}
