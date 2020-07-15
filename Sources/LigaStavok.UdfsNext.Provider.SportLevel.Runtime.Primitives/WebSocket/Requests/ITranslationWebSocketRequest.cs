using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests
{
	public interface ITranslationWebSocketRequest : IWebSocketRequest
	{
		public long TranslationId { get; set; }
	}
}
