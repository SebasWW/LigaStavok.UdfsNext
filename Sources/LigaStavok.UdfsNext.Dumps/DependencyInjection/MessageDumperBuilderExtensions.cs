using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Dumps.FileSystem;
using LigaStavok.UdfsNext.Dumps.SqlServer;

namespace LigaStavok.UdfsNext.DependencyInjection
{
	public static class MessageDumperBuilderExtensions
	{
		public static MessageDumperBuilder ConfigureSqlServerDumping(this MessageDumperBuilder builder, Action<SqlServerDumperOptions> configHandler)
		{
			var options = new SqlServerDumperOptions();
			configHandler.Invoke(options);

			builder.AddMessageDumperFactory(new SqlServerDumperFactory(options));

			return builder;
		}

		public static MessageDumperBuilder ConfigureFileSystemDumping(this MessageDumperBuilder builder, Action<FileSystemDumperOptions> configHandler)
		{
			var options = new FileSystemDumperOptions();
			configHandler.Invoke(options);

			builder.AddMessageDumperFactory(new FileSystemDumperFactory(options));

			return builder;
		}
	}
}
