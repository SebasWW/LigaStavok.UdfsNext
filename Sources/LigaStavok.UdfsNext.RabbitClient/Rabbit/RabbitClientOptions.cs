using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitClientOptions
	{
		public IConnectionFactory ConnectionFactory { get; set; }

		public string Exchange { get; set; }
		public List<string> RoutingKeys { get; set; }

		public string QueueName { get; set; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }

		public Action<object, BasicDeliverEventArgs> OnReceiveDelegate;
	}
}
