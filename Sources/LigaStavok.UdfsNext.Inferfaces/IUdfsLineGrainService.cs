using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Abstractions;
using Orleans.Services;

namespace LigaStavok.UdfsNext.Line
{
	public interface IUdfsLineGrainService : IGrainService
	{
		public Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistration> request);
	}
}
