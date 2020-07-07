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
	internal static class TranslationManagerExtensions
	{

        internal static Task SendSubscribeRequestAsync(
            this TranslationManager subscriptionManager,
            MessageContext<TranslationSubscriptionRequest> messageContext,
            CancellationToken cancellationToken
        )
        {
            Task marketTask, dataTask;

            // Market
            if (messageContext.Message.Subscription.BookedMarket)
            {
                var request = new MarketSubscribeRequest() { TranslationId = messageContext.Message.Id, LastEventNumber = messageContext.Message.State.LastMarketMessageId };
                marketTask = subscriptionManager.SendAsync(messageContext.Next(JsonConvert.SerializeObject(request)), cancellationToken);
            }
            else marketTask = Task.CompletedTask;

            // Data
            if (messageContext.Message.Subscription.BookedData)
            {
                var request = new DataSubscribeRequest() { TranslationId = messageContext.Message.Id, LastEventNumber = messageContext.Message.State.LastDataMessageId};
                dataTask = subscriptionManager.SendAsync(messageContext.Next(JsonConvert.SerializeObject(request)), cancellationToken);
            }
            else dataTask = Task.CompletedTask;

            return Task.WhenAll(dataTask, marketTask);
        }

        internal static Task SendUnsubscribeRequestAsync(
            this TranslationManager subscriptionManager, 
            MessageContext<TranslationUnsubscriptionRequest> messageContext,
            CancellationToken cancellationToken
        )
        {            
            Task marketTask, dataTask;

            // Market
            if (messageContext.Message.Subscription.BookedMarket)
            {
                var request = new MarketUnsubscribeRequest() { TranslationId = messageContext.Message.Id};
                marketTask = subscriptionManager.SendAsync(messageContext.Next(JsonConvert.SerializeObject(request)), cancellationToken);
            }
            else marketTask = Task.CompletedTask;

            // Data
            if (messageContext.Message.Subscription.BookedData)
            {
                var request = new DataUnsubscribeRequest() { TranslationId = messageContext.Message.Id};
                dataTask = subscriptionManager.SendAsync(messageContext.Next(JsonConvert.SerializeObject(request)), cancellationToken);
            }
            else dataTask = Task.CompletedTask;

            return Task.WhenAll(dataTask, marketTask);
        }

        internal static async Task SendPongRequestAsync(this TranslationManager subscriptionManager, MessageContext<PingMessage> context)
        {
            var request = new PongRequest() { Timestamp = context.Message.Timestamp };

            await subscriptionManager.SendAsync(context.Next(JsonConvert.SerializeObject(request)), CancellationToken.None);
        }

        internal static async Task LoginAsync(this TranslationManager subscriptionManager, string userName, string password)
        {
            using (var md5 = MD5.Create())
            {
                var enc = Encoding.UTF8;
                var ts = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                var passwordHashString = BytesToString(md5.ComputeHash(enc.GetBytes(password)));
                var auth = BytesToString(md5.ComputeHash(enc.GetBytes(passwordHashString + ts))) + ":" + ts;

                var request = new LoginRequest() { UserName = userName, AuthKey = auth };

                await subscriptionManager.SendAsync(new MessageContext<string>(JsonConvert.SerializeObject(request)), CancellationToken.None);
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
