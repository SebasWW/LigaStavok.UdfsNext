using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LigaStavok.Threading
{
	public class KeyedSemaphoreSlim<TKey>
	{
		private readonly Dictionary<TKey, KeySemaphoreEntry> keys = new Dictionary<TKey, KeySemaphoreEntry>();

		private object dictionaryLock = new object();

		public int CurrentCount (TKey key)
		{
			return keys[key].CurrentCount;
		}

		private KeySemaphoreEntry RegisterKey(TKey key)
		{
			KeySemaphoreEntry value;

			lock (dictionaryLock)
			{
				if (keys.ContainsKey(key))
					value = keys[key];
				else
				{
					value = new KeySemaphoreEntry();
					keys.Add(key, value);
				}

				value.Count += 1;
			}

			return value;
		}

		internal int UnRegisterKey(TKey key)
		{
			KeySemaphoreEntry value;

			lock (dictionaryLock)
			{
				if (keys.TryGetValue(key, out value))
				{
					if (value.Count == 1)
						keys.Remove(key);
					else
						value.Count -= 1;
				}
			}
			return value.Release();
		}

		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key, KeyedSemaphoreSlimHandle<TKey> parentHandle, TimeSpan timeout, CancellationToken cancellationToken)
		{
			var sync = RegisterKey(key);

			await sync.WaitAsync(timeout, cancellationToken);

			return new KeyedSemaphoreSlimHandle<TKey>(this, key, parentHandle);
		}

		//
		// Summary:
		//     Asynchronously waits to enter the System.Threading.SemaphoreSlim, using a 32-bit
		//     signed integer to measure the time interval.
		//
		// Parameters:
		//   millisecondsTimeout:
		//     The number of milliseconds to wait, System.Threading.Timeout.Infinite (-1) to
		//     wait indefinitely, or zero to test the state of the wait handle and return immediately.
		//
		// Returns:
		//     A task that will complete with a result of true if the current thread successfully
		//     entered the System.Threading.SemaphoreSlim, otherwise with a result of false.
		//
		// Exceptions:
		//   T:System.ObjectDisposedException:
		//     The current instance has already been disposed.
		//
		//   T:System.ArgumentOutOfRangeException:
		//     millisecondsTimeout is a negative number other than -1, which represents an infinite
		//     timeout -or- timeout is greater than System.Int32.MaxValue.
		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key, int millisecondsTimeout)
		{
			return await WaitAsync(key, null,  TimeSpan.FromMilliseconds(millisecondsTimeout), CancellationToken.None);
		}

		//
		// Summary:
		//     Asynchronously waits to enter the System.Threading.SemaphoreSlim, using a 32-bit
		//     signed integer to measure the time interval, while observing a System.Threading.CancellationToken.
		//
		// Parameters:
		//   millisecondsTimeout:
		//     The number of milliseconds to wait, System.Threading.Timeout.Infinite (-1) to
		//     wait indefinitely, or zero to test the state of the wait handle and return immediately.
		//
		//   cancellationToken:
		//     The System.Threading.CancellationToken to observe.
		//
		// Returns:
		//     A task that will complete with a result of true if the current thread successfully
		//     entered the System.Threading.SemaphoreSlim, otherwise with a result of false.
		//
		// Exceptions:
		//   T:System.ArgumentOutOfRangeException:
		//     millisecondsTimeout is a number other than -1, which represents an infinite timeout
		//     -or- timeout is greater than System.Int32.MaxValue.
		//
		//   T:System.ObjectDisposedException:
		//     The current instance has already been disposed.
		//
		//   T:System.OperationCanceledException:
		//     cancellationToken was canceled.
		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			return await WaitAsync(key, null,  TimeSpan.FromMilliseconds(millisecondsTimeout), cancellationToken);
		}

		//
		// Summary:
		//     Asynchronously waits to enter the System.Threading.SemaphoreSlim, while observing
		//     a System.Threading.CancellationToken.
		//
		// Parameters:
		//   cancellationToken:
		//     The System.Threading.CancellationToken token to observe.
		//
		// Returns:
		//     A task that will complete when the semaphore has been entered.
		//
		// Exceptions:
		//   T:System.ObjectDisposedException:
		//     The current instance has already been disposed.
		//
		//   T:System.OperationCanceledException:
		//     cancellationToken was canceled.
		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync (TKey key, CancellationToken cancellationToken)
		{
			return await WaitAsync(key, null,  Timeout.InfiniteTimeSpan, cancellationToken);
		}

		//
		// Summary:
		//     Asynchronously waits to enter the System.Threading.SemaphoreSlim, using a System.TimeSpan
		//     to measure the time interval.
		//
		// Parameters:
		//   timeout:
		//     A System.TimeSpan that represents the number of milliseconds to wait, a System.TimeSpan
		//     that represents -1 milliseconds to wait indefinitely, or a System.TimeSpan that
		//     represents 0 milliseconds to test the wait handle and return immediately.
		//
		// Returns:
		//     A task that will complete with a result of true if the current thread successfully
		//     entered the System.Threading.SemaphoreSlim, otherwise with a result of false.
		//
		// Exceptions:
		//   T:System.ObjectDisposedException:
		//     The current instance has already been disposed.
		//
		//   T:System.ArgumentOutOfRangeException:
		//     millisecondsTimeout is a negative number other than -1, which represents an infinite
		//     timeout -or- timeout is greater than System.Int32.MaxValue.
		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key, TimeSpan timeout)
		{
			return await WaitAsync(key, null,  timeout, CancellationToken.None);
		}

		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key)
		{
			return await WaitAsync(key, null, Timeout.InfiniteTimeSpan, CancellationToken.None);
		}

		public async Task<KeyedSemaphoreSlimHandle<TKey>> WaitAsync(TKey key, KeyedSemaphoreSlimHandle<TKey> parentHandle)
		{
			return await WaitAsync(key, parentHandle, Timeout.InfiniteTimeSpan, CancellationToken.None);
		}

		private class KeySemaphoreEntry : SemaphoreSlim
		{
			public KeySemaphoreEntry() : base(1,1) { }

			public uint Count { get; set; }
		}
	}
}
