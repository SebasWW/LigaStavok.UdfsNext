using LigaStavok.UdfsNext.Provider.SportLevel.State;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationSubscriptionRequest
	{
		public long Id { get; set; }

		public TranslationSubscription Subscription { get; set; }

		public TranslationState State { get; set; }
	}
}
