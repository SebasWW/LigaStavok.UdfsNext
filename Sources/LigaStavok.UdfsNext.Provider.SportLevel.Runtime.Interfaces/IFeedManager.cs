using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface IFeedManager
	{
		Task SendAsync(MessageContext<IWebSocketRequest> messageContext, CancellationToken cancellationToken);
	}
}