using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Dumps.FileSystem
{
	public class FileDumperOptions
	{
		public int MaxDegreeOfParallelism { get; set; }

		public string RootDirectory { get; set; }
	}
}
