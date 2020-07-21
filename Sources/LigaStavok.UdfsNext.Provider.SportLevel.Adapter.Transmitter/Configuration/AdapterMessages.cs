﻿
using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Configuration
{
	public class AdapterMessages
	{
		public MessageConfiguration MarketEvent { get; set; }

		public bool SkipSameCompetitors { get; set; }

		public IEnumerable<long> SkipCompetitors { get; set; }
	}
}
