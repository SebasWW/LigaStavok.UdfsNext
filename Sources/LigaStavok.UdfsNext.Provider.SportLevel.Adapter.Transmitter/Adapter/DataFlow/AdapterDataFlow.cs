using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.Adapter;
using LigaStavok.UdfsNext.Provider.SportLevel.Configuration;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.DataFlow
{
	public class AdapterDataFlow
	{
		private const int maxDegreeOfParallelism = 100;

		private readonly ILogger<AdapterDataFlow> logger;
		private readonly IMessageDumper messageDumper;
		private readonly ITransmitterAdapterHost transmitterHost;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly ITransmitterCommandsFactory transmitterCommandsFactory;
		private readonly AdapterConfiguration adapterConfiguration;

		private readonly TransformManyBlock<MessageContext<Translation>, MessageContext<Translation>> translationCheckBlock;
		private readonly TransformManyBlock<MessageContext<EventsMessage>, MessageContext<EventData, TranslationSubscription>> eventMessageTransformBlock;
		private readonly TransformManyBlock<MessageContext<Translation>, MessageContext<ITransmitterCommand, string>> translationCreateCommandBlock;
		private readonly TransformManyBlock<MessageContext<EventData, TranslationSubscription>, MessageContext<ITransmitterCommand, string>> eventDataCreateCommandBlock;
		private readonly TransformManyBlock<MessageContext<PingMessage>, MessageContext<ITransmitterCommand, string>> pingMessageCreateCommandBlock;
		private readonly TransformManyBlock<MessageContext<ITransmitterCommand, string>, MessageContext<ITransmitterCommand, string>> sendCommandBlock;
		private readonly ActionBlock<MessageContext<ITransmitterCommand, string>> writeDumpBlock;

		public AdapterDataFlow(
			ILogger<AdapterDataFlow> logger,
			IMessageDumper messageDumper,
			ITransmitterAdapterHost transmitterHost,
			TranslationSubscriptionCollection subscriptions,
			ITransmitterCommandsFactory transmitterCommandsFactory,
			AdapterConfiguration adapterConfiguration
		)
		{
			this.logger = logger;
			this.messageDumper = messageDumper;
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
				MessageContext<ITransmitterCommand, string>>(
					TranslationCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2-2
			eventDataCreateCommandBlock
				= new TransformManyBlock<MessageContext<EventData, TranslationSubscription>,
				MessageContext<ITransmitterCommand, string>>(
					EventDataCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2-3
			pingMessageCreateCommandBlock
				= new TransformManyBlock<MessageContext<PingMessage>,
				MessageContext<ITransmitterCommand, string>>(
					PingMessageCreateCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 3
			sendCommandBlock
				= new TransformManyBlock<MessageContext<ITransmitterCommand, string>,
				MessageContext<ITransmitterCommand, string>> (
					SendCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 3
			writeDumpBlock
				= new ActionBlock<MessageContext<ITransmitterCommand, string>>(
					WriteDumpHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			translationCheckBlock.LinkTo(translationCreateCommandBlock);
			eventMessageTransformBlock.LinkTo(eventDataCreateCommandBlock);

			translationCreateCommandBlock.LinkTo(sendCommandBlock);
			eventDataCreateCommandBlock.LinkTo(sendCommandBlock);
			pingMessageCreateCommandBlock.LinkTo(sendCommandBlock);

			sendCommandBlock.LinkTo(writeDumpBlock);
		}

		private void WriteDumpHandler(MessageContext<ITransmitterCommand, string> messageContext)
		{
			try
			{
				messageDumper.Write(
					messageContext.Next(
						new DumpMessage()
						{
							 Source = DumpSource.TO_TRANSMITTER,
							 MessageBody = JsonConvert.SerializeObject(messageContext.Message),
							 MessageType = messageContext.Message.GetType().Name,
							 EventId = messageContext.State
						}
					)
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Dump sending error. ContextId: {messageContext.IncomingId}");
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand, string>> SendCommandHandler(MessageContext<ITransmitterCommand, string> messageContext)
		{
			try
			{
				transmitterHost.SendCommand(messageContext.Message);
				return Enumerable.Repeat(messageContext, 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Sending to transmitter error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<ITransmitterCommand, string>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand, string>> PingMessageCreateCommandHandler(MessageContext<PingMessage> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromPingMessage(messageContext)
					.Select(t => messageContext.NextWithState(t, "Line")).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Process ping message error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<ITransmitterCommand, string>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand, string>> EventDataCreateCommandHandler(MessageContext<EventData, TranslationSubscription> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromEventData(messageContext)
					.Select(t => messageContext.NextWithState(t, messageContext.Message.TranslationId)).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Process event data message error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.TranslationId}");
				return Array.Empty<MessageContext<ITransmitterCommand, string>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand, string>> TranslationCreateCommandHandler(MessageContext<Translation> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateFromTranslation(messageContext)
					.Select(t => messageContext.NextWithState(t, messageContext.Message.Id.ToString())).ToArray();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Process translation message error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
				return Array.Empty<MessageContext<ITransmitterCommand, string>>();
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
				logger.LogError(ex, $"Process event data error. ContextId: {messageContext.IncomingId}");
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
				logger.LogError(ex, $"Translation checking error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message.Id}");
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
