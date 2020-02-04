using System;

namespace LigaStavok.UdfsNext.Abstractions
{
	public interface IUdfsRequest
	{
		string ContextId { get; }

		string SenderId { get; }

		string SenderContextId { get; }

		DateTime UtcDateCreated { get; }
	}

	public interface IUdfsRequest<TMessage> : IUdfsRequest
	{
		TMessage Message { get; }
	}
}