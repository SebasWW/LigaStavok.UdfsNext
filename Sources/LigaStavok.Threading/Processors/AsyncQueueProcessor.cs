using System;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.Threading.Dataflow;

namespace LigaStavok.Threading.Processors
{
	public abstract class AsyncQueueProcessor<T> : IAsyncProcessor<T>
	{
		private CancellationTokenSource processorCancellationTokenSource;
		private readonly AsyncQueue<T> queue = new AsyncQueue<T>();

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await foreach(var message in queue.WithCancellation(stoppingToken))
				{
					try
					{
						var task = OnNext(message);
					}
					catch (Exception ex)
					{
						OnError(ex, message);
					}
				}
			}
		}

		protected abstract Task OnNext(T message);

		public virtual void Enqueue(T message)
		{
			queue.Enqueue(message);
		}

		public virtual Task StartAsync(CancellationToken cancellationToken)
		{
			processorCancellationTokenSource?.Cancel();
			processorCancellationTokenSource = new CancellationTokenSource();

			Task.Run(() => ExecuteAsync(processorCancellationTokenSource.Token));
			return Task.CompletedTask;
		}

		public virtual Task StopAsync(CancellationToken cancellationToken)
		{
			processorCancellationTokenSource?.Cancel();
			return Task.CompletedTask;
		}

		public virtual void OnError(Exception ex, T item) { }
	}
}
