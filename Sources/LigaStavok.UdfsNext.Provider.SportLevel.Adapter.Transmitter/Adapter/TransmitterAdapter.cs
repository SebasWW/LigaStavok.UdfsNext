using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.Adapter.DataFlow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public class TransmitterAdapter : IProviderAdapter
	{
		private readonly ILogger<TransmitterAdapter> logger;
		private readonly AdapterDataFlow adapterDataFlow;

		public TransmitterAdapter(
			ILogger<TransmitterAdapter> logger,
			AdapterDataFlow adapterDataFlow
		)
		{
			this.logger = logger;
			this.adapterDataFlow = adapterDataFlow;
		}

		public Task SendTranslationAsync(MessageContext<Translation> messageContext)
		{
			adapterDataFlow.Post(messageContext);
			return Task.CompletedTask;
		}

		public Task SendEventsAsync(MessageContext<EventsMessage> messageContext)
		{
			adapterDataFlow.Post(messageContext);
			return Task.CompletedTask;
		}

		public Task SendPingAsync(MessageContext<PingMessage> messageContext)
		{
			adapterDataFlow.Post(messageContext);
			return Task.CompletedTask;
		}
	}
}
