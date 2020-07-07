using System;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Orleans
{
	public class ProviderManagerGrainOptions
	{
		public TimeSpan ActivatingProcessDelay { get; set; } = TimeSpan.Zero;
		public TimeSpan ActivatingProcessPeriod { get; set; } = TimeSpan.FromMinutes(1);
	}
}