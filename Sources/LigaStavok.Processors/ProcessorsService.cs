using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.Processors.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.Processors.DependencyInjection
{
	public class ProcessorsService : IHostedService
	{
		private readonly IEnumerable<IProcessor> processors;
		public ProcessorsService(ProcessorsServiceBuilder builder, IServiceProvider serviceProvider)
		{
			processors = builder.Build(serviceProvider);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var processor in processors)
				processor.StartAsync(cancellationToken);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			foreach (var processor in processors)
				processor.StopAsync(cancellationToken);

			return Task.CompletedTask;
		}

	}
}
