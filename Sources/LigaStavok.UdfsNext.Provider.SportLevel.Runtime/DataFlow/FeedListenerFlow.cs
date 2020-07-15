using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow
{
	public class FeedListenerFlow
	{
		private readonly ILogger<FeedListenerFlow> logger;
		private readonly IWebSocketMessageParser webSocketMessageParser;
		private readonly IProviderAdapter providerAdapter;
		private readonly IFeedSubscriber feedSubscriber;
		private readonly TransformManyBlock<MessageContext<string>, MessageContext<object>> parseFeedMessageTransformManyBlock;
		private readonly ActionBlock<MessageContext<object>> messageRouterBlock;
		private readonly ActionBlock<MessageContext<LoginResponseMessage>> startSubscribingActionBlock;
		private readonly ActionBlock<MessageContext<Translation>> translationToAdapterActionBlock;
		private readonly ActionBlock<MessageContext<EventsMessage>> eventsDataToAdapterActionBlock;
		private readonly ActionBlock<MessageContext<PingMessage>> pingToAdapterActionBlock;

		public FeedListenerFlow(
			ILogger<FeedListenerFlow> logger,

			IWebSocketMessageParser webSocketMessageParser,
			IProviderAdapter providerAdapter,
			IFeedSubscriber feedSubscriber
		)
		{
			this.logger = logger;
			this.webSocketMessageParser = webSocketMessageParser;
			this.providerAdapter = providerAdapter;
			this.feedSubscriber = feedSubscriber;

			// 1
			parseFeedMessageTransformManyBlock
				= new TransformManyBlock<MessageContext<string>, MessageContext<object>>(ParseFeedMessageHandler);

			// 2
			messageRouterBlock = new ActionBlock<MessageContext<object>>(MessageRouterHandler);

			// 3-1
			startSubscribingActionBlock
				= new ActionBlock<MessageContext<LoginResponseMessage>>(StartSubscribingHandler);

			// 3-2
			translationToAdapterActionBlock
				= new ActionBlock<MessageContext<Translation>>(TranslationToAdapterHandler);

			// 3-3
			eventsDataToAdapterActionBlock
				= new ActionBlock<MessageContext<EventsMessage>>(EventsDataToAdapterHandler);

			// 3-4
			pingToAdapterActionBlock
				= new ActionBlock<MessageContext<PingMessage>>(PingToAdapterHandler);

			// Building flow
			parseFeedMessageTransformManyBlock.LinkTo(messageRouterBlock);
		}

		public void Post(MessageContext<string> messageContext)
		{
			parseFeedMessageTransformManyBlock.Post(messageContext);
		}

		private IEnumerable<MessageContext<object>> ParseFeedMessageHandler(MessageContext<string> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(webSocketMessageParser.Parse(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Parsing message error.");
				return Array.Empty<MessageContext<object>>();
			}
		}

		private void MessageRouterHandler(MessageContext<object> messageContext)
		{
			try
			{
				switch (messageContext.Message)
				{
					case LoginResponseMessage msg:
						startSubscribingActionBlock.Post(messageContext.Next(msg));
						break;

					case PingMessage msg:
						pingToAdapterActionBlock.Post(messageContext.Next(msg));
						break;

					case EventsMessage msg:
						eventsDataToAdapterActionBlock.Post(messageContext.Next(msg));
						break;

					case SubscribeResponseMessage msg:
						if (msg.Status != "success") throw new Exception("Can't subrcribe translation.");
						break;

					case SubscribeHistorySentMessage msg:
						break;

					case Translation msg:
						translationToAdapterActionBlock.Post(messageContext.Next(msg));
						break;

					default:
						throw new Exception($"Unknown message. {messageContext.Message.GetType().FullName}");
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Message routing error.");
			}
		}

		private void TranslationToAdapterHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				providerAdapter.SendTranslationAsync(messageContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Translation sending error.");
			}
		}

		private void PingToAdapterHandler(MessageContext<PingMessage> messageContext)
		{
			try
			{
				providerAdapter.SendPingAsync(messageContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Ping sending error.");
			}
		}

		private void EventsDataToAdapterHandler(MessageContext<EventsMessage> messageContext)
		{
			try
			{
				providerAdapter.SendEventsAsync(messageContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Events sending error.");
			}
		}

		private void StartSubscribingHandler(MessageContext<LoginResponseMessage> messageContext)
		{
			try
			{
				if (messageContext.Message.Status != "success") throw new AuthenticationException();

				feedSubscriber.StartAsync(CancellationToken.None);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Subscription starting error.");
			}

		}
	}
}
