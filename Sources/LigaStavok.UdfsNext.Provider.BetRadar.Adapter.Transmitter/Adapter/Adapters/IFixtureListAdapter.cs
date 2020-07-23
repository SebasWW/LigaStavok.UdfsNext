﻿using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public interface IFixtureListAdapter
	{
		IEnumerable<ITransmitterCommand> Adapt(MessageContext<FixtureList> messageContext, Language language, LineService lineService);
	}
}