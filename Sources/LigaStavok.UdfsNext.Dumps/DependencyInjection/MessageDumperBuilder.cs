using System.Collections.Generic;
using LigaStavok.UdfsNext.Dumps;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.DependencyInjection
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
