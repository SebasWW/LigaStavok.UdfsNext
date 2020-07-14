using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public interface IFeedManager
	{
		Task SendAsync(MessageContext<string> messageContext, CancellationToken cancellationToken);
	}
}