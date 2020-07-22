using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Providers
{
	public interface IFeedSubscriber<TState>
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
		Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest<TState>> messageContext, Action saveStateAction, CancellationToken cancellationToken);
		Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken);
	}
}