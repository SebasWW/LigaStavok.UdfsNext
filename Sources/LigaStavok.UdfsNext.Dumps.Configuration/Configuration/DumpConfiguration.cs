﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Configuration
{
	public class DumpConfiguration
	{
		public FileSystemDumpConfiguration FileSystem { get; set; }

		public SqlServerDumpConfiguration  SqlServer{ get; set; }
	}
}
