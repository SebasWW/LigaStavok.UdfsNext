using LigaStavok.UdfsNext.Remoting;
using Orleans.Services;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineGrainService : IGrainService
	{
		public Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistrationRequest> request);
	}
}
