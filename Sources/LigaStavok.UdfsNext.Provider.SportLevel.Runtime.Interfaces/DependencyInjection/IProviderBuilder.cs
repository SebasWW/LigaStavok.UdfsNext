using System;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi;
using LigaStavok.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DependencyInjection
{
	public interface IProviderBuilder
	{
		IServiceCollection ServiceCollection { get; }
	}
}