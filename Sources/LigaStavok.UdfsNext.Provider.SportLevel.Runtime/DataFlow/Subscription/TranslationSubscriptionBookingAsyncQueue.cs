using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.Threading.Dataflow;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Subscription
{
	public class TranslationSubscriptionBookingAsyncQueue : AsyncQueue<MessageContext<TranslationSubscriptionBooking>>
	{
	}
}
