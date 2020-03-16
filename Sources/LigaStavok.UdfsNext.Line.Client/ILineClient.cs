using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Services;

namespace LigaStavok.UdfsNext.Line
{
	public interface ILineClient: IGrainServiceClient<IUdfsLineGrainService>, IUdfsLineGrainService
	{
	}
}
