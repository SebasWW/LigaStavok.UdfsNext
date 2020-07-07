using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class TranslationManagerGrain : Grain,  ITranslationManagerGrain
	{
		private readonly IPersistentState<TranslationManagerGrainState> state;
		private readonly ITranslationManager translationManager;

		private TranslationSubscription translationSubscription;

		public TranslationManagerGrain(
			[PersistentState("translationManagerGrainState", "stateStore")] IPersistentState<TranslationManagerGrainState> state,
			ITranslationManager translationManager
		)
		{
			this.state = state;
			this.translationManager = translationManager;
		}

		public Task InitializeAsync(MessageContext<TranslationSubscription> messageContext)
		{
			translationSubscription = messageContext.Message;

			return translationManager.SubscribeAsync(
				messageContext.Next(
					new TranslationSubscriptionRequest()
					{
						Id = GrainReference.GrainIdentity.PrimaryKeyLong,
						State = state.State.Translation,
						Subscription = messageContext.Message
					}
				),
				CancellationToken.None
			);
		}

		public override Task OnActivateAsync()
		{
			return Task.WhenAll(
				base.OnActivateAsync(),
				state.ReadStateAsync()
			);
		}

		public override Task OnDeactivateAsync()
		{
			return Task.WhenAll(
				translationManager.UnsubscribeAsync(
					new MessageContext<TranslationUnsubscriptionRequest>(
						new TranslationUnsubscriptionRequest()
						{
							Id = GrainReference.GrainIdentity.PrimaryKeyLong,
							Subscription = translationSubscription
						}
					),
					CancellationToken.None
				),
				state.WriteStateAsync(),
				base.OnDeactivateAsync()
			);
		}
	}
}
