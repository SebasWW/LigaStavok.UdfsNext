using Microsoft.Extensions.Configuration;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
    public sealed class ApiConfiguration 
    {
        

        public string Host { get; set; }

        public string Key { get; set; }

        
        public bool UseHttps { get; set; }

        public int RetryDelayIntervalMin { get; set; }

        public int RetryCountPerInterval { get; set; }

        public int RecoveryIntervalMinutesMax { get; set; }

        public bool DisableRemoteRequests { get; set; }
    }
}