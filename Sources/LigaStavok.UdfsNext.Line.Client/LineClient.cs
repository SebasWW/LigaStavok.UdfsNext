using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Remoting;
using Orleans.Runtime.Services;

namespace LigaStavok.UdfsNext.Line
{
	public class LineClient : GrainServiceClient<IUdfsLineGrainService>,ILineClient
	{
		public LineClient(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		public Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistrationRequest> request)
		{
			throw new NotImplementedException();
		}
	}
}
