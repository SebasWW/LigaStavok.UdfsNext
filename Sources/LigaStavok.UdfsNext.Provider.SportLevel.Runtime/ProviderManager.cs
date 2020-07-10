using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class ProviderManager : IProviderManager
	{
		private readonly ProviderManagerOptions options;
		private readonly ILogger<ProviderManager> logger;
		private readonly HttpClientManager httpClientManager;
		private readonly IHttpRequestMessageFactory httpRequestMessageFactory;
		private readonly IHttpResponseMessageParser httpResponseMessageParser;
		private CancellationTokenSource cancellationTokenSource;

		private readonly TransformManyBlock<MessageContext<TranslationsRequest>, MessageContext<HttpRequestMessage>> createHttpRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpRequestMessage>, MessageContext<HttpResponseMessage>> execHttpRequestBlock;
		private readonly TransformManyBlock<MessageContext<HttpResponseMessage>, MessageContext<Translation>> parseHttpRequestBlock;

		public ProviderManager(
			ILogger<ProviderManager> logger,
			IOptions<ProviderManagerOptions> options,
			
			HttpClientManager httpClientManager,
			IHttpRequestMessageFactory httpRequestMessageFactory,
			IHttpResponseMessageParser httpResponseMessageParser
		)
		{
			this.options = options.Value;
			this.logger = logger;
			this.httpClientManager = httpClientManager;
			this.httpRequestMessageFactory = httpRequestMessageFactory;
			this.httpResponseMessageParser = httpResponseMessageParser;

			createHttpRequestBlock 
				= new TransformManyBlock<MessageContext<TranslationsRequest>, MessageContext<HttpRequestMessage>>(CreateHttpRequestBlock);

			execHttpRequestBlock
				= new TransformManyBlock<MessageContext<HttpRequestMessage>, MessageContext<HttpResponseMessage>>(ExecHttpRequestBlock);

			parseHttpRequestBlock
				= new TransformManyBlock<MessageContext<HttpResponseMessage>, MessageContext<Translation>>(ParseHttpRequestBlock);

			createHttpRequestBlock.LinkTo(execHttpRequestBlock);
			execHttpRequestBlock.LinkTo(parseHttpRequestBlock);
		}

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				logger.LogDebug("ProviderManager worker running at: {time}", DateTimeOffset.Now);

				try
				{
					// Renewing translations metas
					createHttpRequestBlock.Post(
						new MessageContext<TranslationsRequest>(
							new TranslationsRequest()
							{
								Booking = "booked",
								FromISO8601 = DateTimeOffset.UtcNow.AddHours(-3),
								SportId = 6 // Tennis
							}
						)
					);
					//subscriptionDetails = translations.ToDictionary(key => key.Id, value => new TranslationSubscription() { BookedData = value.BookedData.Value == true, BookedMarket = value.BookedMarket });
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Sending TranslationsRequest error.");
				}

				await Task.Delay(options.MetaRefreshInterval, stoppingToken);
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource = new CancellationTokenSource();

			Task.Run(() => ExecuteAsync(cancellationTokenSource.Token));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			cancellationTokenSource?.Cancel();
			return Task.CompletedTask;
		}

		IEnumerable<MessageContext<HttpRequestMessage>> CreateHttpRequestBlock(MessageContext<TranslationsRequest> messageContext)
		{
			try
			{
				return Enumerable.Repeat(new MessageContext<HttpRequestMessage>(httpRequestMessageFactory.Create(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage building error.");
				return Array.Empty<MessageContext<HttpRequestMessage>>();
			}
		}

		async Task<IEnumerable<MessageContext<HttpResponseMessage>>> ExecHttpRequestBlock(MessageContext<HttpRequestMessage> messageContext)
		{
			try
			{
				return Enumerable.Repeat(messageContext.Next(await httpClientManager.SendAsync(messageContext.Message)), 1);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpRequestMessage execution error.");

				return Array.Empty<MessageContext<HttpResponseMessage>>();
			}
		}

		private async Task<IEnumerable<MessageContext<Translation>>> ParseHttpRequestBlock(MessageContext<HttpResponseMessage> messageContext)
		{
			try
			{
				var response = await httpResponseMessageParser.ParseAsync(messageContext.Message) as TranslationsResponse;
				if (response == null) throw new Exception("Empty response.");

				return response
					.Where(t => t.State != "finished" && t.State != "cancelled")
					.Select(t => messageContext.Next(t));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HttpResponseMessage parsing error.");
				return Array.Empty<MessageContext<Translation>>();
			}
		}

		public async IAsyncEnumerator<MessageContext<Translation>> GetAsyncEnumerator(CancellationToken token = default)
		{
			// Return new elements until cancellationToken is triggered.
			while (true)
			{
				// Make sure to throw on cancellation so the Task will transfer into a canceled state
				token.ThrowIfCancellationRequested();
				yield return await parseHttpRequestBlock.ReceiveAsync(token);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ProviderManager()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion
	}
}
