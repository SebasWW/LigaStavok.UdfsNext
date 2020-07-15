using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;
using Udfs.Transmitter.Messages.Interfaces;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Adapters
{
	public interface ITranslationAdapter
	{
		System.Collections.Generic.IEnumerable<ITransmitterCommand> Adapt(MessageContext<Translation> context);
	}
}