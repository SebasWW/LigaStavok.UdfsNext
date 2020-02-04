using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Services;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineGrainServiceClient: IGrainServiceClient<IUdfsLineGrainService>, IUdfsLineGrainService
	{
	}
}
