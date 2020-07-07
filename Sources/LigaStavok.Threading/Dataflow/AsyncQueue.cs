using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace LigaStavok.Threading.Dataflow
{
    public class AsyncQueue<T> : IAsyncEnumerable<T>
    {
        private readonly BufferBlock<T> bufferBlock = new BufferBlock<T>();

        public void Enqueue(T item) =>
            bufferBlock.Post(item);

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
        {
            // Return new elements until cancellationToken is triggered.
            while (true)
            {
                // Make sure to throw on cancellation so the Task will transfer into a canceled state
                token.ThrowIfCancellationRequested();
                yield return await bufferBlock.ReceiveAsync(token);
            }
        }
    }
}
