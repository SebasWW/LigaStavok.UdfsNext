﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public interface IFeedSubscriber
	{
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
		Task SubscribeAsync(MessageContext<TranslationSubscriptionRequest> messageContext, Action saveStateAction, CancellationToken cancellationToken);
		Task UnsubscribeAsync(MessageContext<TranslationUnsubscriptionRequest> messageContext, CancellationToken cancellationToken);
	}
}