using System.Threading.Tasks;
using LigaStavok.UdfsNext.Abstractions;
using Orleans;

namespace LigaStavok.UdfsNext.Line.Grains
{
	public class UdfsLineEventGrain : Grain, IUdfsLineEventGrain
	{
		public async Task<int> GetLineEvent(int request)
		{
			return  777;
		}
	}
}
