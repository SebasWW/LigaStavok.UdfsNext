using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.Adapter;
using LigaStavok.UdfsNext.Provider.BetRadar.Configuration;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.DataFlow
{
	public class AdapterDataFlow
	{
		private const int maxDegreeOfParallelism = 10;

		private readonly ILogger<AdapterDataFlow> logger;
		private readonly IMessageDumper messageDumper;
		private readonly ITransmitterAdapterHost transmitterHost;
		private readonly TranslationSubscriptionCollection subscriptions;
		private readonly ITransmitterCommandsFactory transmitterCommandsFactory;
		private readonly AdapterConfiguration adapterConfiguration;

		private readonly TransformManyBlock<MessageContext<ApiResponseParsed>, MessageContext<AdaptMessage>> adaptApiMessageBlock;
		private readonly TransformManyBlock<MessageContext<FeedMessageReady>, MessageContext<AdaptMessage>> adaptFeedMessageBlock;
		private readonly TransformManyBlock<MessageContext<AdaptMessage>, MessageContext<ITransmitterCommand>> createTransmitterCommandsBlock;
		private readonly BroadcastBlock<MessageContext<ITransmitterCommand>> broadcastTransmitterCommandBlock;
		private readonly ActionBlock<MessageContext<ITransmitterCommand>> sendCommandBlock;
		private readonly ActionBlock<MessageContext<ITransmitterCommand>> writeDumpBlock;

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
			adaptApiMessageBlock 
				= new TransformManyBlock<MessageContext<ApiResponseParsed>, MessageContext<AdaptMessage>>(
					AdaptApiMessageHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 1-2
			adaptFeedMessageBlock 
				= new TransformManyBlock<MessageContext<FeedMessageReady>, MessageContext<AdaptMessage>>(
					AdaptFeedMessageHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 2
			createTransmitterCommandsBlock = new TransformManyBlock<MessageContext<AdaptMessage>, MessageContext<ITransmitterCommand>>(
					CreateTransmitterCommandsHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);


			// Flow 3
			broadcastTransmitterCommandBlock = new BroadcastBlock<MessageContext<ITransmitterCommand>>(t => t);

			// Flow 4-1
			sendCommandBlock
				= new ActionBlock<MessageContext<ITransmitterCommand>> (
					SendCommandHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			// Flow 4-2
			writeDumpBlock
				= new ActionBlock<MessageContext<ITransmitterCommand>>(
					WriteDumpHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			adaptApiMessageBlock.LinkTo(createTransmitterCommandsBlock);
			adaptFeedMessageBlock.LinkTo(createTransmitterCommandsBlock);

			createTransmitterCommandsBlock.LinkTo(broadcastTransmitterCommandBlock);

			broadcastTransmitterCommandBlock.LinkTo(sendCommandBlock);
			broadcastTransmitterCommandBlock.LinkTo(writeDumpBlock);
		}

		private IEnumerable<MessageContext<AdaptMessage>> AdaptFeedMessageHandler(MessageContext<FeedMessageReady> messageContext)
		{
			try
			{
				return Enumerable.Repeat(
					messageContext.Next(
						new AdaptMessage
						{
							Language = null,
							Message = messageContext.Message.Message,
							LineService = messageContext.Message.Message.Product.ToLineService()
						}
					), 
					1
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Adapting feed message error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<AdaptMessage>>();
			}
		}

		private IEnumerable<MessageContext<AdaptMessage>> AdaptApiMessageHandler(MessageContext<ApiResponseParsed> messageContext)
		{
			try
			{
				return Enumerable.Repeat(
					messageContext.Next(
						new AdaptMessage
						{
							Language = messageContext.Message.Language.Code == "ru" ? Language.Russian : Language.English,
							Message = messageContext.Message.Response,
							LineService = messageContext.Message.ProductType.ToLineService()
						}
					),
					1
				); 
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Adapting api message error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<AdaptMessage>>();
			}
		}

		private IEnumerable<MessageContext<ITransmitterCommand>> CreateTransmitterCommandsHandler(MessageContext<AdaptMessage> messageContext)
		{
			try
			{
				return transmitterCommandsFactory.CreateTransmitterCommands(messageContext).Select(t => messageContext.Next(t));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Adapting api message error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<ITransmitterCommand>>();
			}
		}

		private void WriteDumpHandler(MessageContext<ITransmitterCommand> messageContext)
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
							 EventId = messageContext.Message is IGameEventRelatedCommand command 
								? command.GameEventId.Replace(":", string.Empty)
								: "Line"
						}
					)
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Dump sending error. ContextId: {messageContext.IncomingId}");
			}
		}

		private void SendCommandHandler(MessageContext<ITransmitterCommand> messageContext)
		{
			try
			{
				transmitterHost.SendCommand(messageContext.Message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Sending to transmitter error. ContextId: {messageContext.IncomingId}");
			}
		}

		public void Post(MessageContext<FeedMessageReady> messageContext)
		{
			adaptFeedMessageBlock.Post(messageContext);
		}

		public void Post(MessageContext<ApiResponseParsed> messageContext)
		{
			adaptApiMessageBlock.Post(messageContext);
		}

	}
}
