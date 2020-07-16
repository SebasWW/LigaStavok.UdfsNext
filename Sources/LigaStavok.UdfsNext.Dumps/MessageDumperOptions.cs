using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;

namespace LigaStavok.UdfsNext.Dumps
{
	public class MessageDumperOptions
	{
		public IEnumerable<IMessageDumperFactory> Factories { get; set; }
	}
}
