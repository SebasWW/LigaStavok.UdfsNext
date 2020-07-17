using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitClientService : IHostedService
	{
		private readonly RabbitClientOptions rabbitClientOptions;
		private readonly ILogger logger;

		private IConnection connection;
		private IModel model;
		private QueueDeclareOk queueDeclared;

		public RabbitClientService(
			RabbitClientOptions rabbitClientOptions,
			ILogger logger
		)
		{
			this.rabbitClientOptions = rabbitClientOptions;
			this.logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Initializing rabbit connection...");
			connection = rabbitClientOptions.ConnectionFactory.CreateConnection();
			connection.ConnectionShutdown += OnConnectionShutdown;

			logger.LogInformation("Initializing rabbit channel...");
			model = connection.CreateModel();

			logger.LogInformation("Initializing rabbit queue...");
			queueDeclared = model.QueueDeclare(
				rabbitClientOptions.QueueName,
				rabbitClientOptions.Durable,
				rabbitClientOptions.Exclusive,
				rabbitClientOptions.AutoDelete
			);

			var consumer = new EventingBasicConsumer(model);
			consumer.Received += (o, args) => rabbitClientOptions.OnReceiveDelegate.Invoke(o, args);

			model.BasicConsume(queueDeclared, true, consumer);

			if (!string.IsNullOrWhiteSpace(rabbitClientOptions.Exchange))
			{
				foreach (var key in rabbitClientOptions.RoutingKeys)
					model.QueueBind(queueDeclared, rabbitClientOptions.Exchange, key);
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			if (model != null)
			{
				if (model.IsOpen) model.Close();
				model.Dispose();
			}

			if (connection.IsOpen) connection.Close();
			connection.Dispose();

			return Task.CompletedTask;
		}

		private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
		{
			logger.LogInformation("{Action}", "RabbitConnectionStopped");
		}
	}
}
