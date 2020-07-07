using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class MarketUnsubscribeRequest : UnsubscribeRequest
	{
		private const string FEED_TYPE = "market";

		public MarketUnsubscribeRequest()
		{
			Feed = FEED_TYPE;
		}
	}
}
