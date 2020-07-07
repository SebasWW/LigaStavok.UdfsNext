using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class MarketSubscribeRequest : SubscribeRequest
	{
		private const string FEED_TYPE = "market";

		public MarketSubscribeRequest()
		{
			Feed = FEED_TYPE;
		}
	}
}
