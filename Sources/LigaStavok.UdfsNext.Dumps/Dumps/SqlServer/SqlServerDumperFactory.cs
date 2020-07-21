using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Dumps.SqlServer
{
	public class SqlServerDumperFactory : IMessageDumperFactory
	{
		private readonly SqlServerDumperOptions options;

		public SqlServerDumperFactory(
			SqlServerDumperOptions options
		)
		{
			this.options = options;
		}

		public IMessageDumper Build(ILogger logger)
		{
			return new SqlServerDumper(logger, options);
		}
	}
}
