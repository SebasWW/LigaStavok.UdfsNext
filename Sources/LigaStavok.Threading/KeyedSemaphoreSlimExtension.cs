using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.Threading
{
	public static class KeyedSemaphoreSlimExtension
	{
		public static async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync<TKey>(this KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim, IEnumerable<TKey> keys, 
			TimeSpan timeout, CancellationToken cancellationToken)
		{
			KeyedSemaphoreSlimHandle<TKey> handle = null;

			try
			{
				foreach (var key in keys)
				{
					handle = await keyedSemaphoreSlim.WaitAsync(key, handle, timeout, cancellationToken);
				}
			}
			catch (Exception e)
			{
				handle?.Dispose();
				throw e;
			}

			return handle;
		}

		public static async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync<TKey>(this KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim, IEnumerable<TKey> keys,
			CancellationToken cancellationToken)
		{
			return await WaitAsync(keyedSemaphoreSlim, keys, Timeout.InfiniteTimeSpan, cancellationToken);
		}

		public static async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync<TKey>(this KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim, IEnumerable<TKey> keys, 
			TimeSpan timeout)
		{
			return await WaitAsync( keyedSemaphoreSlim, keys, timeout, CancellationToken.None);
		}

		public static async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync<TKey>(this KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim, IEnumerable<TKey> keys)
		{
			return await WaitAsync(keyedSemaphoreSlim, keys, Timeout.InfiniteTimeSpan, CancellationToken.None);
		}
	}
}
