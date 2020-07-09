//using System;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;

//namespace LigaStavok.Threading
//{
//	public abstract class MultiThreadProcessor<T> : IAsyncProcessor<T>
//	{
//		private readonly ConcurrentQueue<T> itemQueue = new ConcurrentQueue<T>();
//		private readonly ConcurrentDictionary<Guid, Task> taskDictionary = new ConcurrentDictionary<Guid, Task>();
//		private readonly int maxItemCountPerThread;
//		private readonly object syncLock = new object();

//		protected MultiThreadProcessor(int maxItemCountPerThread)
//		{
//			this.maxItemCountPerThread = maxItemCountPerThread;
//		}

//		private async Task ExecuteAsync(Guid processId)
//		{	
//			while (itemQueue.TryDequeue(out var item))
//			{
//				try
//				{
//					var processTask = OnProcess(item);

//					// Adding new thread
//					if (itemQueue.Count / taskDictionary.Count > maxItemCountPerThread)
//					{
//						try
//						{
//							var task = taskDictionary.AddOrUpdate(
//								Guid.NewGuid(),
//								key =>
//								{
//									if (itemQueue.Count / taskDictionary.Count <= maxItemCountPerThread) throw new Exception();
//									return Task.Run(() => ExecuteAsync(key));
//								},
//								(key, value) => throw new InvalidOperationException()
//							);
//						}
//						catch (Exception){} // Concurrency
//					}

//					await processTask;
//				}
//				catch (Exception ex)
//				{
//					OnError(ex, item);
//				}
//			}

//			// Removing Task on exit
//			if (!taskDictionary.TryRemove(processId, out var _))
//				throw new InvalidOperationException(); // Impossible error

//			// Concurency problem
//			if (itemQueue.Count > 0 && taskDictionary.Count == 0)
//				try
//				{
//					var task = taskDictionary.AddOrUpdate(
//						Guid.NewGuid(),
//						key =>
//						{
//							if (itemQueue.Count == 0 || taskDictionary.Count > 0) throw new Exception();
//							return Task.Run(() => ExecuteAsync(key));
//						},
//						(key, value) => throw new InvalidOperationException() // Impossible error
//					);
//				}
//				catch (Exception) { } // Concurrency
//		}

//		protected abstract Task OnProcess(T item);

//		public virtual void Enqueue(T item)
//		{
//			itemQueue.Enqueue(item);

//			// Running first thread
//			if (taskDictionary.Count == 0)
//			{
//				try
//				{
//					taskDictionary.AddOrUpdate(
//						Guid.NewGuid(), 
//						key =>
//						{
//							if (taskDictionary.Count > 0) throw new Exception(); 
//							return Task.Run(() => ExecuteAsync(key));
//						}, 
//						(key, value) => throw new InvalidOperationException()); // Impossible error
//				}
//				catch (Exception){ } // Concurrency
//			}
//		}

//		public virtual void OnError(Exception ex, T item)
//		{

//		}
//	}
//}
