using LigaStavok.UdfsNext.Provider.SportLevel.Api;
using LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class ProviderManager : IProviderManager
	{
		private readonly ProviderManagerOptions options;
		private readonly ILogger<ProviderManager> logger;

		private readonly ICreateTranslationsRequestProcessor createTranslationsRequestProcessor;
		private readonly IExecuteTranslationsRequestProcessor executeTranslationsRequestProcessor;
		private readonly IParseTranslationsResponseProcessor parseTranslationsResponseProcessor;

		private CancellationTokenSource cancellationTokenSource;
		private IDictionary<long, TranslationSubscription> subscriptionDetails;

		public ProviderManager(
			ILogger<ProviderManager> logger,
			IOptions<ProviderManagerOptions> options,
			ICreateTranslationsRequestProcessor createTranslationsRequestProcessor,
			IExecuteTranslationsRequestProcessor executeTranslationsRequestProcessor,
			IParseTranslationsResponseProcessor parseTranslationsResponseProcessor
		)
		{
			this.logger = logger;
			this.createTranslationsRequestProcessor = createTranslationsRequestProcessor;
			this.executeTranslationsRequestProcessor = executeTranslationsRequestProcessor;
			this.parseTranslationsResponseProcessor = parseTranslationsResponseProcessor;
			this.options = options.Value;
		}

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				logger.LogDebug("ProviderManager worker running at: {time}", DateTimeOffset.Now);

				try
				{
					// Renewing translations metas
					createTranslationsRequestProcessor.Enqueue(
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

				await Task.Delay(options.MetaRefreshFrequency, stoppingToken);
			}
		}

		public Task<IDictionary<long, TranslationSubscription>> GetSubscriptionDetailsAsync()
		{
			return Task.FromResult(subscriptionDetails);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource = new CancellationTokenSource();

			createTranslationsRequestProcessor.StartAsync(cancellationToken);
			executeTranslationsRequestProcessor.StartAsync(cancellationToken);
			parseTranslationsResponseProcessor.StartAsync(cancellationToken);

			Task.Run(() => ExecuteAsync(cancellationTokenSource.Token));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			createTranslationsRequestProcessor.StopAsync(cancellationToken);
			executeTranslationsRequestProcessor.StopAsync(cancellationToken);
			parseTranslationsResponseProcessor.StopAsync(cancellationToken);

			cancellationTokenSource?.Cancel();
			return Task.CompletedTask;
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
