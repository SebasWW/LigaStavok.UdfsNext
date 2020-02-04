using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Abstractions
{
	public static class UdfsRequestExtensions
	{
		public static IUdfsRequest<TNextMessage> Next<TMessage,TNextMessage>(this IUdfsRequest<TMessage> request, TNextMessage message)
		{
			return new UdfsRequest<TNextMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}

		public static IUdfsRequest Next<TMessage>(this IUdfsRequest<TMessage> request)
		{
			return new UdfsRequest(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated);
		}

		public static IUdfsRequest<TNextMessage> Next<TNextMessage>(this IUdfsRequest request, TNextMessage message)
		{
			return new UdfsRequest<TNextMessage>(request.ContextId, request.SenderId, request.SenderContextId, request.UtcDateCreated, message);
		}
	}
}
