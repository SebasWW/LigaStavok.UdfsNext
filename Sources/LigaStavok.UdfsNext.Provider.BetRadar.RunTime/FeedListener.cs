//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using LigaStavok.UdfsNext.Provider.BetRadar.DataFlow;
//using LigaStavok.UdfsNext.Provider.BetRadar.State;
//using LigaStavok.UdfsNext.Providers;
//using Microsoft.Extensions.Options;

//namespace LigaStavok.UdfsNext.Provider.BetRadar
//{
//	public class FeedListener : IFeedListener
//	{
//		private readonly FeedListenerFlow feedListenerFlow;
//		private readonly IFeedManager feedManager;
//		private readonly IFeedSubscriber<TranslationState> feedSubscriber;
//		private readonly FeedListenerOptions options;

//		public FeedListener(
//			FeedListenerFlow feedListenerFlow,
//			IFeedManager feedManager,
//			IFeedSubscriber<TranslationState> feedSubscriber,
//			IOptions<FeedListenerOptions> options
//		)
//		{
//			this.feedListenerFlow = feedListenerFlow;
//			this.feedManager = feedManager;
//			this.feedSubscriber = feedSubscriber;
//			this.options = options.Value;
//		}

//		public Task StartAsync(CancellationToken cancellationToken)
//		{

//			return Task.CompletedTask;
//		}

//		public Task StopAsync(CancellationToken cancellationToken)
//		{

//			feedSubscriber.StopAsync(CancellationToken.None);

//			return Task.CompletedTask;
//		}


//		private void WebSocketClient_OnDisconnected(object sender, EventArgs e)
//		{
//			feedSubscriber.StopAsync(CancellationToken.None);
//		}

//		//private void WebSocketClient_OnMessage(object sender, TextMessageReceivedEventArgs e)
//		//{
//		//	feedListenerFlow.Post(new MessageContext<string>(e.MessageText));
//		//}
//	}
//}
