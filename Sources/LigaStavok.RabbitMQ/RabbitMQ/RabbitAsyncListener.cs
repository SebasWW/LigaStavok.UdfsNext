using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public class RabbitAsyncListener : RabbitListener, IAsyncEnumerable<ReadOnlyMemory<byte>>, IRabbitAsyncListener
	{
		private readonly BufferBlock<ReadOnlyMemory<byte>> bufferBlock;

		private readonly RabbitClientOptions options;

		public RabbitAsyncListener(IOptions<RabbitClientOptions> rabbitOptions) : base(rabbitOptions)
		{
			options = rabbitOptions.Value;

			// Buffer
			bufferBlock
				= new BufferBlock<ReadOnlyMemory<byte>>(
					new DataflowBlockOptions()
					{
						EnsureOrdered = options.EnsureOrdered
					}
				 );
		}

		public override void OnMessageReceived(object sender, BasicDeliverEventArgs e)
		{
			bufferBlock.Post(e.Body);
		}

		public async IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(CancellationToken token = default)
		{
			// Return new elements until cancellationToken is triggered.
			while (true)
			{
				// Make sure to throw on cancellation so the Task will transfer into a canceled state
				token.ThrowIfCancellationRequested();
				yield return await bufferBlock.ReceiveAsync(token);
			}
		}

		public IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target)
		{
			return bufferBlock.LinkTo(target);
		}

		public IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target, DataflowLinkOptions linkOptions)
		{
			return bufferBlock.LinkTo(target, linkOptions);
		}

		public override string ToString()
		{
			return bufferBlock.ToString();
		}
		public bool TryReceive(Predicate<ReadOnlyMemory<byte>> filter, out ReadOnlyMemory<byte> item)
		{
			return bufferBlock.TryReceive(filter, out item);
		}
		public bool TryReceiveAll(out IList<ReadOnlyMemory<byte>> items)
		{
			return bufferBlock.TryReceiveAll(out items);
		}
	}
}
