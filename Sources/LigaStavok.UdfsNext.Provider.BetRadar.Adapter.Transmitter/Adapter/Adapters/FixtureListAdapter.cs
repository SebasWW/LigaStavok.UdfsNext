using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Adapters
{
	public class FixtureListAdapter : IFixtureListAdapter
	{
		private readonly IFixtureAdapter fixtureAdapter;

		public FixtureListAdapter(IFixtureAdapter fixtureAdapter)
		{
			this.fixtureAdapter = fixtureAdapter;
		}

		public IEnumerable<ITransmitterCommand> Adapt(MessageContext<FixtureList> messageContext, Language language,
			LineService lineService)
		{
			foreach (var fixture in messageContext.Message.Fixtures)
			{
				foreach (var @event in fixtureAdapter.Adapt(messageContext.Next(fixture), language, lineService))
				{
					yield return @event;
				}
			}

		}
	}
}