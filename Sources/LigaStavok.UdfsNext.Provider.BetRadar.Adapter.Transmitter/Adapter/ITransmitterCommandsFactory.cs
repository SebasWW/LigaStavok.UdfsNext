using System.Collections.Generic;
using LigaStavok.UdfsNext;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter
{
	public interface ITransmitterCommandsFactory
    {
        IEnumerable<ITransmitterCommand> CreateTransmitterCommands(MessageContext<AdaptMessage> messageContext);
    }
}