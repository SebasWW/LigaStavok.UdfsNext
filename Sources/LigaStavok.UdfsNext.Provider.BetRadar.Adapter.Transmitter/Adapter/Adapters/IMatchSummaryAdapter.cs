using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IMatchSummaryAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<MatchSummary> messageContext, Language language, LineService lineService);
	}
}