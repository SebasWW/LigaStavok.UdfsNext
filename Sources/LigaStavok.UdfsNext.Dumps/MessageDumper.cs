using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Dumps
{
	public class MessageDumper : IMessageDumper
	{
		private readonly IMessageDumper[] dumpers;

		public MessageDumper(
			ILogger<MessageDumper> logger,
			IOptions<MessageDumperOptions> options
		)
		{
			dumpers = options.Value.Factories.Select(t => t.Build(logger)).ToArray();
		}

		public void Write(MessageContext<DumpMessage> dumpMessage)
		{
			Array.ForEach(dumpers,t => t.Write(dumpMessage));
		}
	}
}
