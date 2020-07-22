using System;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi;
using LigaStavok.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.BetRadar.DependencyInjection
{
	public interface IProviderBuilder
	{
		IServiceCollection ServiceCollection { get; }
	}
}