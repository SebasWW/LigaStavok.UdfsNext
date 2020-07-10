using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.Processors.DependencyInjection
{
	public class ProcessorsServiceBuilder
	{
		internal readonly IServiceCollection services;
		internal readonly Dictionary<Type, ProcessorConfigurator> processors = new Dictionary<Type, ProcessorConfigurator>();

		public ProcessorsServiceBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public ProcessorConfigurator AddProcessor<TProcessor, TImplementation>() 
			where TProcessor : class, IProcessor 
			where TImplementation : class, TProcessor
		{
			services.AddSingleton<TProcessor, TImplementation>();

			var builder = new ProcessorConfigurator();
			processors.Add(typeof(TProcessor), builder);

			return builder;
		}

		internal IEnumerable<IProcessor> Build(IServiceProvider serviceProvider)
		{
			foreach( var entry in processors)
			{
				var processor = serviceProvider.GetService(entry.Key) as IProcessor;
				yield return processor; 
			}
		}
	}
}
