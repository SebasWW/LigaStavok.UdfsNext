using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.State;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationSubscription
	{
		public TranslationSubscriptionBooking Booking { get; } = new TranslationSubscriptionBooking();
		
		public TranslationState State { get; set; }
	}
}
