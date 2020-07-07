//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages;
//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Requests;
//using LigaStavok.WebSocket;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket
//{
//	public static class WebSocketClientExtensions
//	{
//        public static async Task PongAsync(this IWebsocketClient webSocketClient, PingMessage pingMessage)
//        {
//            var request = new PongRequest() { Timestamp = pingMessage.Timestamp };

//            await webSocketClient.SendAsync(JsonConvert.SerializeObject(request), CancellationToken.None);
//        }

//        public static async Task LoginAsync(this IWebsocketClient webSocketClient, string userName, string password)
//		{
//            using (var md5 = MD5.Create())
//            {
//                var enc = Encoding.UTF8;
//                var ts = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

//                var passwordHashString = BytesToString(md5.ComputeHash(enc.GetBytes(password)));
//                var auth = BytesToString(md5.ComputeHash(enc.GetBytes(passwordHashString + ts))) + ":" + ts;

//                var request = new LoginRequest() { UserName = userName, AuthKey = auth };

//                await webSocketClient.SendAsync(JsonConvert.SerializeObject(request), CancellationToken.None);
//            }
//		}

//        static string BytesToString(byte[] data)
//        {
//            // Create a new Stringbuilder to collect the bytes
//            // and create a string.
//            StringBuilder sBuilder = new StringBuilder();

//            // Loop through each byte of the hashed data 
//            // and format each one as a hexadecimal string.
//            for (int i = 0; i < data.Length; i++)
//            {
//                sBuilder.Append(data[i].ToString("x2"));
//            }

//            // Return the hexadecimal string.
//            return sBuilder.ToString();
//        }
//    }
//}
