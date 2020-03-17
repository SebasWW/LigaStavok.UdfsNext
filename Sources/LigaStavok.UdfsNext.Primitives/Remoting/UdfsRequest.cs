using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Remoting
{
	public class UdfsRequest
	{
		public UdfsRequest(string contextId, string senderId, string senderContextId, DateTime utcDateCreated)
		{
			ContextId = contextId;
			SenderId = senderId;
			SenderContextId = senderContextId;
			UtcDateCreated = utcDateCreated;
		}

		public static UdfsRequest Create(string senderId)
		{
			return new UdfsRequest(Guid.NewGuid().ToString(), senderId, Guid.NewGuid().ToString(), DateTime.UtcNow);
		}

		public string ContextId { get; }

		public string SenderId { get; }

		public string SenderContextId { get; }

		public DateTime UtcDateCreated { get; }
	}

	public class UdfsRequest<T> : UdfsRequest
	{
		public UdfsRequest(string contextId, string senderId, string senderContextId, DateTime utcDateCreated, T message)
			:base(contextId, senderId, senderContextId, utcDateCreated)
		{
			Message = message;
		}

		public T Message { get; }
	}
}
