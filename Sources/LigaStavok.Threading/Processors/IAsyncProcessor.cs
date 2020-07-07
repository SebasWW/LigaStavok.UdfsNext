using Microsoft.Extensions.Hosting;

namespace LigaStavok.Threading
{
	public interface IAsyncProcessor<T> : IHostedService
	{
		void Enqueue(T item);
	}
}