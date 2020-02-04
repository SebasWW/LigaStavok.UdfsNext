using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Abstractions;
using Orleans.Runtime.Services;

namespace LigaStavok.UdfsNext.Line
{
	public class UdfsLineGrainServiceClient : GrainServiceClient<IUdfsLineGrainService>,IUdfsLineGrainServiceClient
	{
		public UdfsLineGrainServiceClient(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		public Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistration> request)
		{
			throw new NotImplementedException();
		}
	}
}
