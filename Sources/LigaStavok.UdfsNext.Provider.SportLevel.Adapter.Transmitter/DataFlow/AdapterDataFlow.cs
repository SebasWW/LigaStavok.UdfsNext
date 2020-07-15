using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Microsoft.Extensions.Logging;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow
{
	public class AdapterDataFlow
	{
		private const int maxDegreeOfParallelism = 100;

		private readonly ILogger<AdapterDataFlow> logger;
		private readonly ITransmitterHost transmitterHost;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly ITransmitterCommandsFactory transmitterCommandsFactory;
		private readonly AdapterConfiguration adapterConfiguration;

		private readonly TransformManyBlock<MessageContext<Translation>, MessageContext<Translation>> translationCheckBlock;
		private readonly TransformManyBlock<MessageContext<EventsMessage>, MessageContext<EventData, TranslationSubscription>> eventMessageTransformBlock;
		private readonly TransformManyBlock<MessageContext<Translation>, MessageContext<ITransmitterCommand>> translationCreateCommandBlock;
		private readonly TransformManyBlock<MessageContext<EventData,TranslationSubscription>, MessageContext<ITransmitterCommand>> eventDataCreateCommandBlock;
		private readonly TransformManyBlock<MessageContext<PingMessage>, MessageContext<ITransmitterCommand>> pingMessageCreateCommandBlock;
		private readonly ActionBlock<MessageContext<ITransmitterCommand>> sendCommandBlock;

		public AdapterDataFlow(
			ILogger<AdapterDataFlow> logger,
			ITransmitterHost transmitterHost,
			TranslationSubscriptionCollection subscriptions,
			ITransmitterCommandsFactory transmitterCommandsFactory,
			AdapterConfiguration adapterConfiguration
		)
		{
			this.logger = logger;
			this.transmitterHost = transmitterHost;
			this.subscriptions = subscriptions;
			this.transmitterCommandsFactory = transmitterCommandsFactory;
			this.adapterConfiguration = adapterConfiguration;

			// Flow 1-1
			translationCheckBlock
				= new TransformManyBlock<MessageContext<Translation>,
				MessageContext<Translation>>(
					TranslationCheckHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 1-2
			eventMessageTransformBlock
				= new TransformManyBlock<MessageContext<EventsMessage>,
				MessageContext<EventData, TranslationSubscription>>(
					EventMessageTransformHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2-1
			translationCreateCommandBlock
				= new TransformManyBlock<MessageContext<Translation>,
				MessageContext<ITransmitterCommand>>(
					TranslationCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2-2
			eventDataCreateCommandBlock
				= new TransformManyBlock<MessageContext<EventData, TranslationSubscription>,
				MessageContext<ITransmitterCommand>>(
					EventDataCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2-3
			pingMessageCreateCommandBlock
				= new TransformManyBlock<MessageContext<PingMessage>,
				MessageContext<ITransmitterCommand>>(
					PingMessageCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 3
			sendCommandBlock
				= new ActionBlock<MessageContext<ITransmitterCommand>>(
					SendCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			translationCheckBlock.LinkTo(translationCreateCommandBlock);
			eventMessageTransformBlock.LinkTo(eventDataCreateCommandBlock);

			translationCreateCommandBlock.LinkTo(sendCommandBlock);
			eventDataCreateCommandBlock.LinkTo(sendCommandBlock);
			pingMessageCreateCommandBlock.LinkTo(sendCommandBlock);
		}

		private void SendCommandHandler(MessageContext<ITransmitterCommand> messageContext)
		{
			try
			{
				transmitterHost.SendCommand(messageContext.Message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Sending to transmitter error.");
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand>> PingMessageCreateCommandHandler(MessageContext<PingMessage> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromPingMessage(messageContext).Select(t => messageContext.Next(t)).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Process ping message error.");
				return Array.Empty< MessageContext<ITransmitterCommand>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand>> EventDataCreateCommandHandler(MessageContext<EventData, TranslationSubscription> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromEventData(messageContext).Select(t => messageContext.Next(t)).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Process event data message error.");
				return Array.Empty< MessageContext<ITransmitterCommand>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand>> TranslationCreateCommandHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromTranslation(messageContext).Select(t => messageContext.Next(t)).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Process translation message error.");
				return Array.Empty< MessageContext<ITransmitterCommand>>();
			}
		}

		private IEnumerable<MessageContext<EventData, TranslationSubscription>> EventMessageTransformHandler(MessageContext<EventsMessage> messageContext)
		{
			List<MessageContext<EventData, TranslationSubscription>> list 
				= new List<MessageContext<EventData, TranslationSubscription>>();

			try
			{
				long oldTranslationId = 0;
				TranslationSubscription subscription = null;

				foreach (var item in messageContext.Message.Events)
				{
					if (long.TryParse(item.TranslationId, out long translationId))
					{
						if ((translationId == oldTranslationId && subscription != null) || subscriptions.TryGetValue(translationId, out subscription))
						{
							oldTranslationId = translationId;

							list.Add(messageContext.NextWithState(item, subscription));
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Process event data error.");
			}

			return list;
		}

		private IEnumerable<MessageContext<Translation>> TranslationCheckHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				var translation = messageContext.Message;

				if (adapterConfiguration.Messages.SkipSameCompetitors && translation.AwayTeamId == translation.HomeTeamId)
				{

					logger.LogWarning($"Translation validation is failed. Id: {messageContext.Message.Id}, Reason: AwayTeamId=HomeTeamId");
					return Enumerable.Empty<MessageContext<Translation>>();
				}

				if (adapterConfiguration.Messages.SkipCompetitors.Any(t => t == translation.AwayTeamId || t == translation.HomeTeamId))
				{

					logger.LogWarning($"Translation validation is failed. Id: {messageContext.Message.Id}, Reason: SkippedTeamId");
					return Enumerable.Empty<MessageContext<Translation>>();
				}

				return Enumerable.Repeat(messageContext, 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Translation checking error.");
				return Enumerable.Empty<MessageContext<Translation>>();
			}
		}

		public void Post(MessageContext<PingMessage> messageContext)
		{
			pingMessageCreateCommandBlock.Post(messageContext);
		}

		public void Post(MessageContext<Translation> messageContext)
		{
			translationCheckBlock.Post(messageContext);
		}

		public void Post(MessageContext<EventsMessage> messageContext)
		{
			eventMessageTransformBlock.Post(messageContext);
		}
	}
}
