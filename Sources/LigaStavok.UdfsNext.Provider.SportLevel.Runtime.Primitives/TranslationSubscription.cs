using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.State;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationSubscription
	{
		private readonly Action saveStateAction;

		public TranslationSubscription(Action saveStateAction)
		{
			this.saveStateAction = saveStateAction;
		}

		public TranslationSubscriptionBooking Booking { get; } = new TranslationSubscriptionBooking();
		
		public TranslationState PersistableState { get; set; }

		public int MetaHash { get; set; }

		public void SaveState()
		{
			saveStateAction?.Invoke();
		}
	}
}
