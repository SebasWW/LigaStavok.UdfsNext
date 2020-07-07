using System;

namespace LigaStavok.WebSocket
{
	public class TextMessageReceivedEventArgs : EventArgs
	{
		public string MessageText { get; set; }
	}
}
