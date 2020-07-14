using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class FeedSubscriberGrain : Grain,  IFeedSubscriberGrain
	{
		private readonly IPersistentState<FeedSubscriberGrainState> state;
		private readonly IFeedSubscriber feedSubscriber;

		public FeedSubscriberGrain(
			[PersistentState("translationGrainState", "stateStore")] IPersistentState<FeedSubscriberGrainState> state,
			IFeedSubscriber feedSubscriber
		)
		{
			this.state = state;
			this.feedSubscriber = feedSubscriber;
		}

		public Task InitializeAsync()
		{
			return Task.CompletedTask;
		}

		public override async Task OnActivateAsync()
		{
			await state.ReadStateAsync();

			await feedSubscriber.SubscribeAsync(
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
			await feedSubscriber.UnsubscribeAsync(
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
