using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.State
{
	public class TranslationState
	{
		public long LastMarketMessageId { get; set; }

		public long LastDataMessageId { get; set; }

		public ConcurrentDictionary<int, Score> MatchScore { get; set; }

		public ConcurrentDictionary<string, TranslationMarket> Markets { get; set; }
	}
}
