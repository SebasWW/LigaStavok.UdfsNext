using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext
{

	public class MessageContext<TMessage> : MessageContext
	{
		public MessageContext(Guid incomingId, DateTimeOffset receivedOn, TMessage message) : base(incomingId, receivedOn)
		{
			Message = message;
		}

		public MessageContext(TMessage message)
		{
			Message = message;
		}

		public TMessage Message { get; }
	}
	public class MessageContext 
	{
		public MessageContext(Guid incomingId, DateTimeOffset receivedOn)
		{
			IncomingId = incomingId;
			ReceivedOn = receivedOn;
		}

		public MessageContext()
		{
			IncomingId = Guid.NewGuid();
			ReceivedOn = DateTimeOffset.UtcNow;
		}

		public Guid IncomingId { get; }
		public DateTimeOffset ReceivedOn { get; }

		public MessageContext<TNextMessage> Next<TNextMessage>(TNextMessage message)
		{
			return new MessageContext<TNextMessage>(IncomingId, ReceivedOn, message);
		}
	}
}
