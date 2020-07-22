using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public class FeedListenerService : IHostedService
	{
		private readonly IFeedListener feedListener;

		public FeedListenerService(
			IFeedListener feedListener
		)
		{
			this.feedListener = feedListener;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			feedListener.StartAsync(cancellationToken);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			feedListener.StartAsync(cancellationToken);
			return Task.CompletedTask;
		}
	}
}
