using System;
using System.Security;

namespace LigaStavok.WebSocket
{
	public class WebSocketClientOptions
	{
		public Uri Uri { get; set; }

		public bool UseDefaultCredentials { get; set; } = true;
		public string UserName { get; set; }
		public SecureString Password { get; set; }

		public int BufferSize { get; set; } = 4096;
		public TimeSpan FailureReconnectDelay { get; set; } = TimeSpan.FromSeconds(5);
	}
}