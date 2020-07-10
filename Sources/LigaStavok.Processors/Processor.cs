using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LigaStavok.Processors
{
	public abstract class Processor<T> : Processor, IProcessor<T>
	{
		private readonly BufferBlock<T> bufferBlock = new BufferBlock<T>();

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var message = await bufferBlock.ReceiveAsync(stoppingToken);
				
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

		protected abstract Task OnNext(T message);

		public virtual void Enqueue(T message)
		{
			bufferBlock.Post(message);
		}

		public virtual void OnError(Exception ex, T item) { }
	}


	public abstract class Processor : IProcessor
	{
		protected CancellationTokenSource processorCancellationTokenSource;
		
		public virtual Task StartAsync(CancellationToken cancellationToken)
		{
			processorCancellationTokenSource?.Cancel();
			processorCancellationTokenSource = new CancellationTokenSource();

			Task.Run(() => ExecuteAsync(processorCancellationTokenSource.Token));
			return Task.CompletedTask;
		}

		public ProcessorOptions Options { get; } = new ProcessorOptions();

		protected abstract Task ExecuteAsync(CancellationToken token);

		public virtual Task StopAsync(CancellationToken cancellationToken)
		{
			processorCancellationTokenSource?.Cancel();
			return Task.CompletedTask;
		}
	}
}
