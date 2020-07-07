using System;

namespace LigaStavok.Threading
{
	public class KeyedSemaphoreSlimHandle<TKey> : IDisposable
	{
		private readonly KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim;
		private readonly TKey key;
		private readonly KeyedSemaphoreSlimHandle<TKey> parentHandle;

		internal KeyedSemaphoreSlimHandle(KeyedSemaphoreSlim<TKey> keyedSemaphoreSlim, TKey key, KeyedSemaphoreSlimHandle<TKey> parentHandle)  {
			this.keyedSemaphoreSlim = keyedSemaphoreSlim;
			this.key = key;
			this.parentHandle = parentHandle;
		}

		public int CurrentCount { get => keyedSemaphoreSlim.CurrentCount(key); }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					keyedSemaphoreSlim.UnRegisterKey(key);
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}

			parentHandle?.Dispose();
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~KeySemaphoreHandle()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
