using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface IProviderAdapter
	{
		Task SendEventsAsync(MessageContext<EventsMessage> messageContext);
		Task SendTranslationAsync(MessageContext<Translation> messageContext);
		Task SendPingAsync(MessageContext<PingMessage> messageContext);
	}
}
