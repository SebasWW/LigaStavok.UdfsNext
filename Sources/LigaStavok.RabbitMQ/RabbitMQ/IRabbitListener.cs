using System;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public interface IRabbitListener
	{
		event EventHandler OnMessageReceivedEvent;

		void Dispose();
		Task StartAsync(CancellationToken cancellationToken);
		Task StopAsync(CancellationToken cancellationToken);
	}
}