using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Dumps.DependencyInjection
{
	public class MessageDumperBuilder
	{
		private readonly IServiceCollection services;
		private readonly List<IMessageDumperFactory> factories = new List<IMessageDumperFactory>();

		public MessageDumperBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public void AddMessageDumperFactory(IMessageDumperFactory messageDumperFactory)
		{
			factories.Add(messageDumperFactory);
		}

		public void Build()
		{
			services.AddSingleton<IMessageDumper, MessageDumper>();
			services.Configure<MessageDumperOptions>(
				options =>
				{
					options.Factories = factories;
				}
			);
		}
	}
}
