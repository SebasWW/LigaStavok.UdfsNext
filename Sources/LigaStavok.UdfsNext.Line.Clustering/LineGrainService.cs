using System;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Remoting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using Orleans.Core;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Line
{
	[Reentrant]
	public class LineGrainService : GrainService, IUdfsLineGrainService
	{
		readonly IGrainFactory GrainFactory;
		private ILogger<LineGrainService> logger;

		public LineGrainService(IGrainIdentity id, Silo silo, ILoggerFactory loggerFactory, IGrainFactory grainFactory) : base(id, silo, loggerFactory)
		{
			GrainFactory = grainFactory;
			logger = loggerFactory.CreateLogger<LineGrainService>();
		}

		public async Task<int> RegisterOrGetEvent(UdfsRequest<LineEventRegistrationRequest> request)
		{
			return 222;
		}
	}
}
