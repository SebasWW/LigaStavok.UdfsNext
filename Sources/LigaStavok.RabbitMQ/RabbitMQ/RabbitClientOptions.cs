using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Security;
using System.Text;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitClientOptions
	{

		public string Host { get; set; }

		public string VirtualHost { get; set; }

		public int Port { get; set; }

		public bool SslEnabled { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string Queue { get; set; }

		public string Exchange { get; set; }

		public Collection<string> RoutingKeys { get; set; }

		public bool Durable { get; set; }

		public bool Exclusive { get; set; }

		public bool AutoDelete { get; set; }

		public TimeSpan ReconnectionTimeout { get; set; }

		public TimeSpan AfterConnectionDownTimeout { get; set; } = TimeSpan.FromSeconds(3);

		public bool EnsureOrdered { get;  set; }

		public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);

		public SslPolicyErrors AcceptablePolicyErrors { get;  set; } =
			SslPolicyErrors.RemoteCertificateNotAvailable
			| SslPolicyErrors.RemoteCertificateNameMismatch
			| SslPolicyErrors.RemoteCertificateChainErrors;

		public bool AutomaticRecoveryEnabled { get; set; } = true;
	}
}
