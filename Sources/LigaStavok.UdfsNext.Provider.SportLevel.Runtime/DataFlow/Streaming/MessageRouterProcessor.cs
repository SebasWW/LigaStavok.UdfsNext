using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.DataFlow.Streaming
{
	public class MessageRouterProcessor
	{

		//public override async Task OnProcess(MessageContext<string> messageContext)
		//{
		//	var message = webSocketMessageParser.Parse(messageContext.Message);

		//	switch (message)
		//	{
		//		case LoginResponseMessage msg:

		//			await Task.WhenAll(subscriptions.Select(t => this.SendSubscribeRequestAsync(messageContext.Next(t.Value), CancellationToken.None)).ToArray());

		//			break;
		//		case PingMessage msg:

		//			// Response to feed
		//			await this.SendPongRequestAsync(messageContext.Next(msg));

		//			// Alive to db
		//			await providerAdapter.PingAsync(messageContext.Next(msg));
		//			break;

		//		case EventsMessage msg:

		//			// Sending market events only
		//			foreach (var item in msg.Events)
		//			{
		//				providerAdapter.EventAsync(messageContext.Next(item));
		//			}
		//			break;

		//		case SubscribeResponseMessage msg:

		//			// Todo
		//			break;

		//		case SubscribeHistorySentMessage msg:

		//			// Todo
		//			break;

		//		case Translation msg:

		//			_adapterActor.Tell(messageContext.Next(msg));
		//			break;

		//		default:
		//			throw new ArgumentException("Unexpected message received.");
		//	}
		//}
	}
}
