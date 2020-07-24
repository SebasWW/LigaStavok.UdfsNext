using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace LigaStavok.UdfsNext.Line.Providers.RabbitClient
{
	public interface IRabbitAsyncListener : IRabbitListener
	{
		IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(CancellationToken token = default);
		IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target);
		IDisposable LinkTo(ITargetBlock<ReadOnlyMemory<byte>> target, DataflowLinkOptions linkOptions);
		bool TryReceive(Predicate<ReadOnlyMemory<byte>> filter, out ReadOnlyMemory<byte> item);
		bool TryReceiveAll(out IList<ReadOnlyMemory<byte>> items);
	}
}