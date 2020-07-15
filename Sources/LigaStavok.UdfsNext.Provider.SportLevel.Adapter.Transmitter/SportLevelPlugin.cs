using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using Udfs.Common;
using Udfs.Common.Actor;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{

	//public class SportLevelPlugin : IPlugin
	//{
	//	private readonly IConfigurationRoot _config;
	//	private readonly SportLevelPluginInjector sportLevelPluginInjector;

	//	public SportLevelPlugin(IConfigurationRoot config
	//	//	, SportLevelPluginInjector sportLevelPluginInjector
	//	)
	//	{
	//		_config = config;
	//		//this.sportLevelPluginInjector = sportLevelPluginInjector;
	//	}

	//	public string Name => "SportLevel";
	//	public TimeSpan TimeNeededToStop => TimeSpan.FromSeconds(100);

	//	public void RegisterDependencies(Container container)
	//	{
	//		//container.RegisterInstance(sportLevelPluginInjector);
	//		container.RegisterInstance(new ActorMetadata<RootActor>(Name));
	//		//container.Register<TransmitterRootActor>();
	//		//container.RegisterInstance(ActorPaths.Root);
	//		//container.RegisterInstance(ActorPaths.Dump);

	//		//container.RegisterSingleton<IMarketEventAdapter, MarketEventAdapter>();

	//		//var configuration = _config.GetSection("sport-level").Get<SportLevelConfiguration>();
	//		//container.RegisterInstance(configuration.Adapter);

	//		//container.RegisterInstance(_config.GetRequiredSection<DumpingOptions>("dumps"));
	//	}
	//}
}
