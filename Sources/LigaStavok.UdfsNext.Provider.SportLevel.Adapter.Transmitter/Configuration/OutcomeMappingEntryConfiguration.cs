using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Configuration
{
	public class OutcomeMappingEntryConfiguration
	{
		public int GroupId { get; set; }

		public IEnumerable<int> Values { get; set; }
	}
}
