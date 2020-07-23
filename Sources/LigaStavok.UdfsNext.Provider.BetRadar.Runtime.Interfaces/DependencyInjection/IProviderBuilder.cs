using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection
{
	public interface IProviderBuilder
	{
		IServiceCollection ServiceCollection { get; }
	}
}