using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Remoting
{
	public class UdfsResponse
	{
		public UdfsResponse(string contextId, string senderId, string senderContextId, DateTime utcDateCreated)
		{
			ContextId = contextId;
			SenderId = senderId;
			SenderContextId = senderContextId;
			UtcDateCreated = utcDateCreated;
		}

		public string ContextId { get; }

		public string SenderId { get; }

		public string SenderContextId { get; }

		public DateTime UtcDateCreated { get; }
	}

	public class UdfsResponse<T> : UdfsResponse
	{
		public UdfsResponse(string contextId, string senderId, string senderContextId, DateTime utcDateCreated, T message)
			: base(contextId, senderId, senderContextId, utcDateCreated)
		{
			Message = message;
		}

		public T Message { get; }
	}
}
