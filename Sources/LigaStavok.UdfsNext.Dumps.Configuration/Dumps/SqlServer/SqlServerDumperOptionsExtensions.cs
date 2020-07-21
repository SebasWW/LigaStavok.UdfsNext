using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Configuration;

namespace LigaStavok.UdfsNext.Dumps.SqlServer
{
	public static class SqlServerDumperOptionsExtensions
	{
		public static SqlServerDumperOptions Configure(this SqlServerDumperOptions options, SqlServerDumpConfiguration configuration)
		{
			options.BatchSize = configuration.BatchSize;
			options.ConnectionString = configuration.ConnectionString;
			options.DestinationTableName = configuration.TableName;
			options.MaxDegreeOfParallelism = configuration.MaxDegreeOfParallelism;
			options.ServiceId = configuration.ServiceId;

			return options;
		}
	}
}
