using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter
{
	public interface ITransmitterCommandsFactory
	{
		IEnumerable<ITransmitterCommand> CreateFromEventData(MessageContext<EventData, TranslationSubscription> messageContext);
		IEnumerable<ITransmitterCommand> CreateFromPingMessage(MessageContext<PingMessage> messageContext);
		IEnumerable<ITransmitterCommand> CreateFromTranslation(MessageContext<Translation> messageContext);
	}
}