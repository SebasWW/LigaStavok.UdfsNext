using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using LigaStavok.UdfsNext.Providers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.BetRadar.DataFlow
{
	public class ProviderManagerFlow //: IAsyncEnumerable<MessageContext<Translation>>
	{
		int maxDegreeOfParallelism = 10;

		private readonly ILogger<ProviderManagerFlow> logger;
		private readonly IMessageDumper messageDumper;
		private readonly HttpClientManager httpClientManager;
		private readonly IHttpRequestMessageFactory httpRequestMessageFactory;
		private readonly ITranslationDistributer translationDistributer;

		private readonly TransformManyBlock<MessageContext<RequestFixtureChanges>, MessageContext<HttpRequestMessage>> createHttpRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpRequestMessage>, MessageContext<HttpResponseMessage>> execHttpRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpResponseMessage>, MessageContext<string>> parseHttpResponseBlock;
		private readonly BroadcastBlock<MessageContext<string>> broadcastResponseBlock;
		private readonly ActionBlock<MessageContext<string>> dumpResponseBlock;
		private readonly TransformManyBlock<MessageContext<string>, MessageContext<FixtureChangeList>> deserializeResponseBlock;
		private readonly TransformManyBlock<MessageContext<FixtureChangeList>, MessageContext<long>> parsingIdBlock;
		private readonly ActionBlock<MessageContext<long>> translationDistributerBlock;

		public ProviderManagerFlow(
			ILogger<ProviderManagerFlow> logger,
			IMessageDumper messageDumper,

			HttpClientManager httpClientManager,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			ITranslationDistributer translationDistributer,
			ProviderManagerState providerManagerState
		)
		{
			this.logger = logger;
			this.messageDumper = messageDumper;
			this.httpClientManager = httpClientManager;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.translationDistributer = translationDistributer;

			createHttpRequestBlock
				= new TransformManyBlock<MessageContext<RequestFixtureChanges>, MessageContext<HttpRequestMessage>>(
					CreateHttpRequestHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			execHttpRequestBlock
				= new TransformManyBlock<MessageContext<HttpRequestMessage>, MessageContext<HttpResponseMessage>>(
					ExecHttpRequestHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			parseHttpResponseBlock
				= new TransformManyBlock<MessageContext<HttpResponseMessage>, MessageContext<string>>(
					ParseHttpResponseHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			broadcastResponseBlock = new BroadcastBlock<MessageContext<string>>(t => t);

			dumpResponseBlock
				= new ActionBlock<MessageContext<string>>(
					DumpResponseHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			deserializeResponseBlock
				= new TransformManyBlock<MessageContext<string>, MessageContext<FixtureChangeList>>(
					DeserializeResponseHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			parsingIdBlock
				= new TransformManyBlock<MessageContext<FixtureChangeList>, MessageContext<long>>(
					ParsingIdHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			translationDistributerBlock
				= new ActionBlock<MessageContext<long>>(
					TranslationDistributerHandler,
					new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }
				);

			createHttpRequestBlock.LinkTo(execHttpRequestBlock);
			execHttpRequestBlock.LinkTo(parseHttpResponseBlock);
			parseHttpResponseBlock.LinkTo(broadcastResponseBlock);

			broadcastResponseBlock.LinkTo(dumpResponseBlock);
			broadcastResponseBlock.LinkTo(deserializeResponseBlock);

			deserializeResponseBlock.LinkTo(parsingIdBlock);
			parsingIdBlock.LinkTo(translationDistributerBlock);
		}

		public void Post(MessageContext<RequestFixtureChanges> messageContext)
		{
			createHttpRequestBlock.Post(messageContext);
		}

		IEnumerable<MessageContext<HttpRequestMessage>> CreateHttpRequestHandler(MessageContext<RequestFixtureChanges> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(httpRequestMessageFactory.Create(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Building httpRequestMessage error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<HttpRequestMessage>>();
			}
		}

		async Task<IEnumerable<MessageContext<HttpResponseMessage>>> ExecHttpRequestHandler(MessageContext<HttpRequestMessage> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(await httpClientManager.SendAsync(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Execution httpRequestMessage error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<HttpResponseMessage>>();
			}
		}

		private async Task<IEnumerable<MessageContext<string>>> ParseHttpResponseHandler(MessageContext<HttpResponseMessage> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next( await messageContext.Message.Content.ReadAsStringAsync()), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Parsing httpResponseMessage error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<string>>();
			}
		}

		private void DumpResponseHandler(MessageContext<string> messageContext)
		{
			try
			{
				messageDumper.Write(
					messageContext.Next(
						new DumpMessage()
						{
							EventId = "Line",
							Source = DumpSource.FROM_API,
							MessageType = "FixtureChangeList",
							MessageBody = messageContext.Message
						}
					)
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Dumping reponse error. ContextId: {messageContext.IncomingId}");
			}
		}

		private IEnumerable<MessageContext<FixtureChangeList>> DeserializeResponseHandler(MessageContext<string> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(FixtureChangeList.Parse(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Deserializing response error. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<FixtureChangeList>>();
			}
		}

		private IEnumerable<MessageContext<long>> ParsingIdHandler(MessageContext<FixtureChangeList> messageContext)
		{
			try
			{

				return messageContext.Message.FixtureChanges
					.Select(t => t.SportEventId)
					.SelectMany(
						t =>
						{
							try
							{
								var arr = t.Split(':');
								if (arr.Length == 3)
								{
									if (long.TryParse(arr[2], out var id)) return Enumerable.Repeat( messageContext.Next(id), 1);
								}

								logger.LogError( $"Can't parse translation id. ContextId: {messageContext.IncomingId}, SportEventId: {t}");
								return Array.Empty<MessageContext<long>>();
							}
							catch (Exception ex)
							{
								logger.LogError(ex, $"Parsing translation id error. ContextId: {messageContext.IncomingId}, SportEventId: {t}");
								return Array.Empty<MessageContext<long>>();
							}
						}
					);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Parsing translation ids. ContextId: {messageContext.IncomingId}");
				return Array.Empty<MessageContext<long>>();
			}
		}

		private Task TranslationDistributerHandler(MessageContext<long> messageContext)
		{
			try
			{
				return translationDistributer.Distribute(messageContext.Message);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Distributing translation error. ContextId: {messageContext.IncomingId}, TranslationId: {messageContext.Message}");
				return Task.CompletedTask;
			}
		}
	}
}
