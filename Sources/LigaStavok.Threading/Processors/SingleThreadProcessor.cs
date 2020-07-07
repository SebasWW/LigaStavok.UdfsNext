using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.Threading
{
	public abstract class SingleThreadProcessor<T> : IAsyncProcessor<T>
	{
		private CancellationTokenSource processorCancellationTokenSource;
		private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

		private CancellationTokenSource delayCancellationTokenSource;

		private async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				while (queue.TryPeek(out _)) // Sync with AddItem
				{

					while (queue.TryDequeue(out var item))
					{
						try
						{
							await OnProcess(item);
						}
						catch (Exception ex)
						{
							OnError(ex, item);
						}
					}

					delayCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
				}

				try
				{
					await Task.Delay(-1, delayCancellationTokenSource.Token);
				}
				catch (Exception)
				{
					// Cancel
				}

				delayCancellationTokenSource = null;
			}
		}

		protected abstract Task OnProcess(T item);

		public virtual void Enqueue(T item)
		{
			queue.Enqueue(item);
			delayCancellationTokenSource?.Cancel();
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

		public virtual void OnError(Exception ex, T item)
		{

		}
	}
}
