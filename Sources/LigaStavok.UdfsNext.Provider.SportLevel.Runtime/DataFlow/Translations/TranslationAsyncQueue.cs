using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.Threading.Dataflow;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Messages;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Translations
{
	public class TranslationAsyncQueue : AsyncQueue<MessageContext<Translation>>
	{
	}
}
