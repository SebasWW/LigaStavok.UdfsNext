using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LigaStavok.Threading.Dataflow;
using LigaStavok.Threading.Processors;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Streaming
{
	public class MessageParserProcessor : AsyncQueueProcessor<MessageContext<string>>, IMessageParserProcessor
	{
		public MessageParserProcessor()
		{
		}

		protected override Task OnNext(MessageContext<string> message)
		{
			throw new NotImplementedException();
		}
	}
}
