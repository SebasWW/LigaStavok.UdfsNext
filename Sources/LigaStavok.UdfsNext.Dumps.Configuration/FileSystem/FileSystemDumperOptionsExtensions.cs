using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Dumps.Configuration;

namespace LigaStavok.UdfsNext.Dumps.FileSystem
{
	public static class FileSystemDumperOptionsExtensions
	{
		public static FileSystemDumperOptions Configure(this FileSystemDumperOptions options, FileSystemDumpConfiguration configuration)
		{
			options.MaxDegreeOfParallelism = configuration.MaxDegreeOfParallelism;
			options.RootDirectory = configuration.RootDirectory;

			return options;
		}
	}
}
