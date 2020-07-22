using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.State;
using LigaStavok.UdfsNext.Providers;
using LigaStavok.UdfsNext.Providers.Orleans;
using Orleans;
using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class FeedSubscriberGrain : Grain,  IFeedSubscriberGrain
	{
		private readonly IPersistentState<FeedSubscriberGrainState> state;
		private readonly IFeedSubscriber<TranslationState> feedSubscriber;
		private bool needSaveState;

		public const string STORAGE_NAME = "stateStore";

		public FeedSubscriberGrain(
			[PersistentState("translationGrainState", STORAGE_NAME)] IPersistentState<FeedSubscriberGrainState> state,
			IFeedSubscriber<TranslationState> feedSubscriber
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
			if (state.State.Translation == null) state.State.Translation
					= new TranslationState()
					{
						MatchScore = new System.Collections.Concurrent.ConcurrentDictionary<int, Score>(),
						Markets = new System.Collections.Concurrent.ConcurrentDictionary<string, TranslationMarket>()
					};

			// Save state timer
			RegisterTimer(SaveStateTimer, null, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));

			// Subscribe translation
			await feedSubscriber.SubscribeAsync(
				new MessageContext<TranslationSubscriptionRequest<TranslationState>>(
					new TranslationSubscriptionRequest<TranslationState>() 
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
			try
			{

				await state.WriteStateAsync();
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}
