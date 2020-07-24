﻿//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using LigaStavok.UdfsNext.Dumps;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace LigaStavok.UdfsNext.Provider.BetRadar
//{
//	public class FeedManager : IFeedManager
//	{
//		private readonly ILogger<FeedManager> logger;
//		private readonly IWebSocketClient webSocketClient;
//		private readonly IMessageDumper messageDumper;

//		public FeedManager(
//			ILogger<FeedManager> logger,
//			IWebSocketClient webSocketClient,
//			IMessageDumper messageDumper
//		)
//		{
//			this.logger = logger;
//			this.webSocketClient = webSocketClient;
//			this.messageDumper = messageDumper;
//		}

//		public Task SendAsync(MessageContext<IWebSocketRequest> messageContext, CancellationToken cancellationToken)
//		{
//			var message = JsonConvert.SerializeObject(messageContext.Message);

//			var task = webSocketClient.SendAsync(message, cancellationToken);

//			messageDumper.Write(messageContext.Next(
//					new DumpMessage()
//					{
//						EventId = (messageContext.Message as ITranslationWebSocketRequest)?.TranslationId.ToString()  ?? "Line",
//						Source = DumpSource.TO_FEED,
//						MessageType = messageContext.Message.GetType().Name,
//						MessageBody = message
//					}
//				)
//			);

//			return task;
//		}
//	}
//}
