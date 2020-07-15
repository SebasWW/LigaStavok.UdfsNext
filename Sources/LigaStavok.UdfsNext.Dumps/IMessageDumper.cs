using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Dumps
{
	public interface IMessageDumper
	{
		void Write(MessageContext<DumpMessage> dumpMessage);
	}
}
