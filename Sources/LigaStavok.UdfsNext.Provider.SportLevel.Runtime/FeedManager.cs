using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.WebSocket;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class FeedManager : IFeedManager
	{
		private readonly ILogger<FeedManager> logger;
		private readonly IWebSocketClient webSocketClient;

		public FeedManager(
			ILogger<FeedManager> logger,
			IWebSocketClient webSocketClient
		)
		{
			this.logger = logger;
			this.webSocketClient = webSocketClient;
		}

		public Task SendAsync(MessageContext<string> context, CancellationToken cancellationToken)
		{
			return webSocketClient.SendAsync(context.Message, cancellationToken);
		}
	}
}
