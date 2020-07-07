using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{		
	public interface IWebSocketRequest
	{
		string MsgId { get; }

		string MsgType { get; }
	}
}
