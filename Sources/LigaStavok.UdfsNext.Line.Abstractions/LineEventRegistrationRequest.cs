using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Line
{
	public class LineEventRegistrationRequest
	{
		public string ExternalId { get; set; }

		public string Name { get; set; }

		public Sports Sport { get; set; }

		public string ProviderId { get; set; }
	}
}
