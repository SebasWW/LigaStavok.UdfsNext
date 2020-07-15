using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Udfs.Common.Messages;
using Udfs.Transmitter;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TransmitterHostService : IHostedService
	{
		private readonly ITransmitterHost transmitterHost;

		public TransmitterHostService(
			ITransmitterHost transmitterHost	
		)
		{
			this.transmitterHost = transmitterHost;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			transmitterHost.StartAsync(cancellationToken);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			transmitterHost.StopAsync(cancellationToken);
			return Task.CompletedTask;
		}
	}
}
