using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests;
using Newtonsoft.Json;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public static class IFeedManagerExtensions
	{

        public static Task SendMarketSubscribeRequestAsync(
            this IFeedManager subscriptionManager,
            MessageContext<TranslationSubscriptionRequest> messageContext,
            FeedSubscriberOptions options,
            CancellationToken cancellationToken
        )
        {         
            var request = new MarketSubscribeRequest() 
            { 
                TranslationId = messageContext.Message.Id, 
                LastEventNumber = messageContext.Message.State.LastMarketMessageId,
            };
            request.Parameters.Margin = options.Margin;

            return subscriptionManager.SendAsync(messageContext.Next<IWebSocketRequest>(request), cancellationToken);
        }

        public static Task SendDataSubscribeRequestAsync(
            this IFeedManager subscriptionManager,
            MessageContext<TranslationSubscriptionRequest> messageContext,
            CancellationToken cancellationToken
        )
        {
            var request = new DataSubscribeRequest() { TranslationId = messageContext.Message.Id, LastEventNumber = messageContext.Message.State.LastDataMessageId };
            return subscriptionManager.SendAsync(messageContext.Next<IWebSocketRequest>(request), cancellationToken);
        }

        public static Task SendMarketUnsubscribeRequestAsync(
            this IFeedManager subscriptionManager, 
            MessageContext<TranslationUnsubscriptionRequest> messageContext,
            CancellationToken cancellationToken
        )
        {                
            var request = new MarketUnsubscribeRequest() { TranslationId = messageContext.Message.Id};
            return subscriptionManager.SendAsync(messageContext.Next<IWebSocketRequest>(request), cancellationToken);
        }

        public static Task SendDataUnsubscribeRequestAsync(
            this IFeedManager subscriptionManager,
            MessageContext<TranslationUnsubscriptionRequest> messageContext,
            CancellationToken cancellationToken
        )
        {
            var request = new DataUnsubscribeRequest() { TranslationId = messageContext.Message.Id };
            return subscriptionManager.SendAsync(messageContext.Next<IWebSocketRequest>(request), cancellationToken);
        }

        public static async Task SendPongRequestAsync(this IFeedManager subscriptionManager, MessageContext<PingMessage> context)
        {
            var request = new PongRequest() { Timestamp = context.Message.Timestamp };
            await subscriptionManager.SendAsync(context.Next<IWebSocketRequest>(request), CancellationToken.None);
        }

        public static async Task LoginAsync(this IFeedManager subscriptionManager, string userName, string password)
        {
            using (var md5 = MD5.Create())
            {
                var enc = Encoding.UTF8;
                var ts = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                var passwordHashString = BytesToString(md5.ComputeHash(enc.GetBytes(password)));
                var auth = BytesToString(md5.ComputeHash(enc.GetBytes(passwordHashString + ts))) + ":" + ts;

                var request = new LoginRequest() { UserName = userName, AuthKey = auth };

                await subscriptionManager.SendAsync(new MessageContext<IWebSocketRequest>(request), CancellationToken.None);
            }
        }

        static string BytesToString(byte[] data)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
