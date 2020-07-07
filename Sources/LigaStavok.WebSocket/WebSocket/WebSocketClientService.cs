using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.WebSocket
{
	public class WebSocketClientService : IHostedService
	{
		private readonly IWebsocketClient webSocketClient;
		private CancellationTokenSource CancellationTokenSource;

		public WebSocketClientService(IWebsocketClient webSocketClient)
		{
			this.webSocketClient = webSocketClient;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			CancellationTokenSource?.Cancel();
			CancellationTokenSource = new CancellationTokenSource();

			webSocketClient.Start(CancellationTokenSource.Token); 
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			CancellationTokenSource.Cancel();
			return Task.CompletedTask;
		}
	}
}
