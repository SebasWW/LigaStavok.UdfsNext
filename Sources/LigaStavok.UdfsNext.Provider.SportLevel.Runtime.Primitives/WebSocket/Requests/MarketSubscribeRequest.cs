﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class MarketSubscribeRequest : SubscribeRequest
	{
		private const string FEED_TYPE = "market";

		public MarketSubscribeRequest()
		{
			Feed = FEED_TYPE;
		}

		[JsonProperty("params")]
		public MarketSubscribeRequestParameters Parameters { get; } = new MarketSubscribeRequestParameters();

	}
}
