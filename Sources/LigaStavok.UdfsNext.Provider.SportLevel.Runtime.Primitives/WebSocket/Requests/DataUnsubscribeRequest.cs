using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public class DataUnsubscribeRequest : UnsubscribeRequest
	{
		private const string FEED_TYPE = "data";

		public DataUnsubscribeRequest()
		{
			Feed = FEED_TYPE;
		}
	}
}
