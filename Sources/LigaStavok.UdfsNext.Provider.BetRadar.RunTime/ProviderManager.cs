using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.BetRadar.DataFlow;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
using LigaStavok.UdfsNext.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public class ProviderManager : IProviderManager
	{
		private readonly ProviderManagerOptions options;
		private readonly ILogger<ProviderManager> logger;
		private readonly ProviderManagerFlow providerManagerFlow;
		private CancellationTokenSource cancellationTokenSource;

		public ProviderManager(
			ILogger<ProviderManager> logger,
			IOptions<ProviderManagerOptions> options,
			ProviderManagerFlow providerManagerFlow
		)
		{
			this.options = options.Value;
			this.logger = logger;
			this.providerManagerFlow = providerManagerFlow;
		}

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				logger.LogDebug("ProviderManager worker running at: {time}", DateTimeOffset.Now);

				try
				{
					// Renewing translations metas
					providerManagerFlow.Post(
						new MessageContext<TranslationsRequest>(
							new TranslationsRequest()
							{
								//Booking = "booked",
								//FromISO8601 = DateTimeOffset.UtcNow.AddHours(-3),
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

		//public IAsyncEnumerator<MessageContext<Translation>> GetAsyncEnumerator(CancellationToken token = default)
		//{
		//	// Return new elements until cancellationToken is triggered.
		//	return providerManagerFlow.GetAsyncEnumerator(token);

		//}

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
