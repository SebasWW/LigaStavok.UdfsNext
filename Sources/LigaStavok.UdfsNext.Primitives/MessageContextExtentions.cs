using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext
{
	public static class MessageContextExtentions
	{
		public static MessageContext<TNextMessage, TState> NextWithState<TNextMessage, TState>(this MessageContext messageContext, TNextMessage message, TState state)
		{
			return new MessageContext<TNextMessage, TState>(messageContext.IncomingId, messageContext.ReceivedOn, message, state);
		}
	}
}
