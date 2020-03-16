﻿using System.Threading.Tasks;
using LigaStavok.UdfsNext.Line;
using LigaStavok.UdfsNext.Remoting;
using Orleans;

namespace LigaStavok.UdfsNext.Line.Provider
{
	public interface IUdfsLineProviderGrain : IGrainWithIntegerKey
	{
		Task<UdfsResponse<LineEvent>> GetAsync(UdfsRequest request);
	}
}
