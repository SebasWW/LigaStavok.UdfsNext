using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class TranslationSubscriberGrain : Grain,  ITranslationSubscriberGrain
	{
		private readonly IPersistentState<TranslationSubscriberGrainState> state;
		private readonly IFeedManager translationManager;

		public TranslationSubscriberGrain(
			[PersistentState("translationGrainState", "stateStore")] IPersistentState<TranslationSubscriberGrainState> state,
			IFeedManager translationManager
		)
		{
			this.state = state;
			this.translationManager = translationManager;
		}

		public Task InitializeAsync()
		{
			return Task.CompletedTask;
		}

		public override async Task OnActivateAsync()
		{
			await state.ReadStateAsync();

			await translationManager.SubscribeAsync(
				new MessageContext<TranslationSubscriptionRequest>(
					new TranslationSubscriptionRequest() 
					{
						Id = GrainReference.GrainIdentity.PrimaryKeyLong,
						State = state.State.Translation
					}
				),
				CancellationToken.None
			);
		}

		public override async Task OnDeactivateAsync()
		{
			await translationManager.UnsubscribeAsync(
				new MessageContext<TranslationUnsubscriptionRequest>(
					new TranslationUnsubscriptionRequest()
					{
						Id = GrainReference.GrainIdentity.PrimaryKeyLong
					}
				),
				CancellationToken.None
			);

			await state.WriteStateAsync();
		}
	}
}
