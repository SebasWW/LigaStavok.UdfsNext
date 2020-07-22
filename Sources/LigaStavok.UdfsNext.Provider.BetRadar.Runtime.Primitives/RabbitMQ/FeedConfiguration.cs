using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    public class FeedConfiguration : BetRadarBaseConfig
    {
        public FeedConfiguration(IConfigurationRoot configurationRoot) 
            : base(configurationRoot, "feed")
        {

        }

        public string Host
            => GetConfigurationValue<string>();

        public string VirtualHost
			=> GetConfigurationValue<string>();

        public int Port
            => GetConfigurationValue<int>();

		public bool Ssl
			=> GetConfigurationValue<bool>();

		public string UserName
            => GetConfigurationValue<string>();

		public string Password
			=> GetConfigurationValue<string>();

		public bool AllowMessagesFromApi
			=> GetConfigurationValue<bool>();

		public string Queue
			=> GetConfigurationValue<string>();

		public string Exchange
			=> GetConfigurationValue<string>();

		public ImmutableHashSet<string> RoutingKeys => ConfigSection.GetSection("routing-keys")
			.GetChildren().Select(x => x.Value).ToImmutableHashSet();

		public bool Durable 
			=> GetConfigurationValue<bool>();

		public bool Exclusive
			=> GetConfigurationValue<bool>();

		public bool AutoDelete
			=> GetConfigurationValue<bool>();


		public int ReconnectionTimeoutSeconds
			=> GetConfigurationValue<int>();


		public int AfterConnectionDownTimeoutSeconds
			=> GetConfigurationValue<int>();
	}
}