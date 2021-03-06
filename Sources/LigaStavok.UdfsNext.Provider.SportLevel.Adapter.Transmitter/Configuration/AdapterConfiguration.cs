﻿using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Configuration
{
	public class AdapterConfiguration
	{
		public AdapterMessages Messages { get; set; }

		public SpecifiersConfiguration Specifiers { get; set; }

		public IEnumerable<OutcomeMappingEntryConfiguration> OutcomeMapping { get; set; }
	}
}
