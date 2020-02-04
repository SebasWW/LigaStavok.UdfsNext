using System.Threading.Tasks;
using LigaStavok.UdfsNext.Abstractions;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Line
{
	public class UdfsLineGrainService : GrainService, IUdfsLineGrainService
	{
		public async Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistration> request)
		{
			return 222;
		}
	}
}
