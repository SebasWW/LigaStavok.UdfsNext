using Orleans.Runtime;

namespace LigaStavok.UdfsNext.Line.Providers.BetRadar
{
	public class BetRadarGrainService : GrainService, IUdfsLineProviderGrainService
	{
		public string Id => "betradar";

		public string Name => "Betradar line provider";
	}
}
