using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.WebSocket
{
	public class WebSocketClientService : IHostedService
	{
		private readonly IWebSocketClient webSocketClient;
		private CancellationTokenSource stoppingTokenSource;

		public WebSocketClientService(IWebSocketClient webSocketClient)
		{
			this.webSocketClient = webSocketClient;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			stoppingTokenSource?.Cancel();
			stoppingTokenSource = new CancellationTokenSource();

			webSocketClient.Start(stoppingTokenSource.Token); 
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			stoppingTokenSource.Cancel();
			return Task.CompletedTask;
		}
	}
}
