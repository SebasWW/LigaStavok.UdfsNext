using Microsoft.Extensions.Hosting;

namespace LigaStavok.Processors
{
	public interface IProcessor<T> : IProcessor
	{
		void Enqueue(T item);
	}

	public interface IProcessor : IHostedService
	{
	}
}