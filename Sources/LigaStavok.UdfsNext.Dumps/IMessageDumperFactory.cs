using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Dumps
{
	public interface IMessageDumperFactory
	{
		IMessageDumper Build(ILogger logger);
	}
}
