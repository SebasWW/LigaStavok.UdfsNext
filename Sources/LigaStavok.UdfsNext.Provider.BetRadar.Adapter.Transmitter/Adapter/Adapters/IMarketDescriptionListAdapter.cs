using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IMarketDescriptionListAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<MarketDescriptionList> messageContext, Language language, LineService lineService);
	}
}