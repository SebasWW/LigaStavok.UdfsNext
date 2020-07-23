using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter
{
	public class AdaptMessage
    {
        public Language Language { get; set; }

        public object Message { get; set; }

		public LineService LineService { get; set; }
    }
}