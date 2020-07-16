using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Dumps.FileSystem
{
	public class FileSystemDumperFactory : IMessageDumperFactory
	{
		private readonly FileSystemDumperOptions options;

		public FileSystemDumperFactory(
			FileSystemDumperOptions options
		)
		{
			this.options = options;
		}

		public IMessageDumper Build(ILogger logger)
		{
			return new FileSystemDumper(logger, options);
		}
	}
}
