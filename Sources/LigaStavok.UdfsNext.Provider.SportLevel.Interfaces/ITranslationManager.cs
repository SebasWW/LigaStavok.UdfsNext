using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface ITranslationManager
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);

		Task ExecuteAsync(CancellationToken stoppingToken);

		Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, CancellationToken cancellationToken);
		Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken);
	}
}