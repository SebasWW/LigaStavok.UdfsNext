using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Dumps.FileSystem
{
	public class FileSystemDumperOptions
	{
		public int MaxDegreeOfParallelism { get; set; } = 100;

		public string RootDirectory { get; set; }
	}
}
