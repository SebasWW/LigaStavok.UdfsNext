using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Dumps.Configuration
{
	public class FileSystemDumpConfiguration
	{ 
		public bool Enabled { get; set; }

		public int MaxDegreeOfParallelism { get; set; } = 100;

		public string RootDirectory { get; set; }
	}
}
