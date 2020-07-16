using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using Orleans;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class FeedSubscriberGrain : Grain,  IFeedSubscriberGrain
	{
		private readonly IPersistentState<FeedSubscriberGrainState> state;
		private readonly IFeedSubscriber feedSubscriber;
		private bool needSaveState;

		public const string STORAGE_NAME = "stateStore";

		public FeedSubscriberGrain(
			[PersistentState("translationGrainState", STORAGE_NAME)] IPersistentState<FeedSubscriberGrainState> state,
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

			if (state.State == null) state.State = new FeedSubscriberGrainState();
			if (state.State.Translation == null) state.State.Translation = new TranslationState();

			RegisterTimer(SaveStateTimer, null, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));


			await feedSubscriber.SubscribeAsync(
				new MessageContext<TranslationSubscriptionRequest>(
					new TranslationSubscriptionRequest() 
					{
						Id = GrainReference.GrainIdentity.PrimaryKeyLong,
						State = state.State.Translation
					}
				),
				() => needSaveState = true,
				CancellationToken.None
			);
		}

		private Task SaveStateTimer(object arg)
		{
			if (needSaveState)
			{
				needSaveState = false;
				return state.WriteStateAsync();
			}

			return Task.CompletedTask;
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
