using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class DataSubscribeRequest : SubscribeRequest
	{
		private const string FEED_TYPE = "data";

		public DataSubscribeRequest()
		{
			Feed = FEED_TYPE;
		}
	}
}
