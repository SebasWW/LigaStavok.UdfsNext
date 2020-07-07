using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.State;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationUnsubscriptionRequest
	{
		public long Id { get; set; }

		public TranslationSubscription Subscription { get; set; }
	}
}
