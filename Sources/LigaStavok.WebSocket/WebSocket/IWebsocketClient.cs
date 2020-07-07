using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.WebSocket
{
	public interface IWebsocketClient : IDisposable
	{
		event EventHandler<TextMessageReceivedEventArgs> OnMessage;
		event EventHandler<EventArgs> OnConnected;
		event EventHandler<EventArgs> OnDisconnected;

		void Start(CancellationToken cancellationToken);

		bool IsConnected { get; }

		Task SendAsync(string text, CancellationToken cancellationToken);
	}
}
