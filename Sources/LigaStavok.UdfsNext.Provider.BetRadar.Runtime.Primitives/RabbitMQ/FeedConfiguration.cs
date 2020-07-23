using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    public class FeedConfiguration 
    {
        public string Host { get; set; }

        public string VirtualHost { get; set; }

		public int Port { get; set; }

		public bool Ssl { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public bool AllowMessagesFromApi { get; set; }

		public string Queue { get; set; }

		public string Exchange { get; set; }

		public IEnumerable<string> RoutingKeys { get; set; }

		public bool Durable { get; set; }

		public bool Exclusive { get; set; }

		public bool AutoDelete { get; set; }

		public int ReconnectionTimeoutSeconds { get; set; }

		public int AfterConnectionDownTimeoutSeconds { get; set; }
	}
}