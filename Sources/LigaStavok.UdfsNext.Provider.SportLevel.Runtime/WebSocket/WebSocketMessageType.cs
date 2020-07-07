using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket
{
    public static class WebSocketMessageType
    {

        public const string Ping = "ping";
        public const string LoginResponse = "login_response";
        public const string SubscribeResponse = "subscribe_response";
        public const string UnsubscribeResponse = "unsubscribe_response";
        public const string SubscribeHistorySent = "subscribe_history_sent";
        public const string Events = "events";

        public const string Unknown = "unknown";
    }
}
