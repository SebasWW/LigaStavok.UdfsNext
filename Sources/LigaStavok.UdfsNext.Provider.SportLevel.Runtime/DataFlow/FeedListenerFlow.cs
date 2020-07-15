using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow
{
	public class FeedListenerFlow
	{
		private readonly ILogger<FeedListenerFlow> logger;
		private readonly IMessageDumper messageDumper;
		private readonly IWebSocketMessageParser webSocketMessageParser;
		private readonly IProviderAdapter providerAdapter;
		private readonly IFeedSubscriber feedSubscriber;
		private readonly TransformManyBlock<MessageContext<string>, MessageContext<object, string>> parseFeedMessageTransformManyBlock;
		private readonly ActionBlock<MessageContext<object, string>> messageRouterBlock;
		private readonly ActionBlock<MessageContext<LoginResponseMessage>> startSubscribingActionBlock;
		private readonly ActionBlock<MessageContext<Translation>> translationToAdapterActionBlock;
		private readonly ActionBlock<MessageContext<EventsMessage>> eventsDataToAdapterActionBlock;
		private readonly ActionBlock<MessageContext<PingMessage>> pingToAdapterActionBlock;

		public FeedListenerFlow(
			ILogger<FeedListenerFlow> logger,
			IMessageDumper messageDumper,
			IWebSocketMessageParser webSocketMessageParser,
			IProviderAdapter providerAdapter,
			IFeedSubscriber feedSubscriber
		)
		{
			this.logger = logger;
			this.messageDumper = messageDumper;
			this.webSocketMessageParser = webSocketMessageParser;
			this.providerAdapter = providerAdapter;
			this.feedSubscriber = feedSubscriber;

			// 1
			parseFeedMessageTransformManyBlock
				= new TransformManyBlock<MessageContext<string>, MessageContext<object,string>>(ParseFeedMessageHandler);

			// 2
			messageRouterBlock 
				= new ActionBlock<MessageContext<object, string>>(MessageRouterHandler);

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

		private IEnumerable<MessageContext<object, string>> ParseFeedMessageHandler(MessageContext<string> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.NextWithState(webSocketMessageParser.Parse(messageContext.Message), messageContext.Message), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Parsing message error.");

				messageDumper.Write(
					messageContext.Next(
						new DumpMessage()
						{
							EventId = "Line",
							SourceType = "FromFeed",
							MessageBody = messageContext.Message,
							MessageType = "Unkuown"
						}
					)
				);

				return Array.Empty<MessageContext<object, string>>();
			}
		}

		private void MessageRouterHandler(MessageContext<object, string> messageContext)
		{
			try
			{
				var dumpMessage = new DumpMessage()
				{
					EventId = "Line",
					SourceType = "FromFeed",
					MessageBody = messageContext.State,
					MessageType = messageContext.Message.GetType().Name
				};

				switch (messageContext.Message)
				{
					case LoginResponseMessage msg:
						startSubscribingActionBlock.Post(messageContext.Next(msg));
						messageDumper.Write(messageContext.Next(dumpMessage));
						break;

					case PingMessage msg:
						pingToAdapterActionBlock.Post(messageContext.Next(msg));
						messageDumper.Write(messageContext.Next(dumpMessage));
						break;

					case EventsMessage msg:
						eventsDataToAdapterActionBlock.Post(messageContext.Next(msg));
						messageDumper.Write(messageContext.Next(dumpMessage));

						break;

					case SubscribeResponseMessage msg:
						messageDumper.Write(messageContext.Next(dumpMessage));
						if (msg.Status != "success") throw new Exception("Can't subrcribe translation.");
						break;

					case SubscribeHistorySentMessage msg:
						messageDumper.Write(messageContext.Next(dumpMessage));
						break;

					case Translation msg:
						translationToAdapterActionBlock.Post(messageContext.Next(msg));
						dumpMessage.EventId = msg.Id.ToString();
						messageDumper.Write(messageContext.Next(dumpMessage));
						break;

					default:
						messageDumper.Write(messageContext.Next(dumpMessage));
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
