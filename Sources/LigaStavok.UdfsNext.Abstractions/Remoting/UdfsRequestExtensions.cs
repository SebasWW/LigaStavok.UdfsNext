using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Remoting
{
	public static class UdfsRequestExtensions
	{
		public static UdfsRequest<TNextMessage> NextRequest<TMessage, TNextMessage>(this UdfsRequest<TMessage> request, TNextMessage message)
		{
			return new UdfsRequest<TNextMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}

		public static UdfsRequest NextRequest<TMessage>(this UdfsRequest<TMessage> request)
		{
			return new UdfsRequest(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated);
		}

		public static UdfsRequest<TNextMessage> NextRequest<TNextMessage>(this UdfsRequest request, TNextMessage message)
		{
			return new UdfsRequest<TNextMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}

		public static UdfsResponse<TResultMessage> ToResponse<TMessage, TResultMessage>(this UdfsRequest<TMessage> request, TResultMessage message)
		{
			return new UdfsResponse<TResultMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}

		public static UdfsResponse<TResultMessage> ToResponse<TResultMessage>(this UdfsRequest request, TResultMessage message)
		{
			return new UdfsResponse<TResultMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}

		public static UdfsResponse ToResponse<TMessage>(this UdfsRequest<TMessage> request)
		{
			return new UdfsResponse(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated);
		}

		public static UdfsResponse ToResponse(this UdfsRequest request)
		{
			return new UdfsResponse(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated);
		}
	}
}
