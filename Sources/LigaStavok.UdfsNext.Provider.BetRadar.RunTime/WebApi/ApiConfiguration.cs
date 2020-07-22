using Microsoft.Extensions.Configuration;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
    public sealed class ApiConfiguration : BetRadarBaseConfig
    {
        public ApiConfiguration(IConfigurationRoot configurationRoot)
            : base(configurationRoot, "api")
        {

        }

        public string Host
            => GetConfigurationValue<string>();

        public string Key
            => GetConfigurationValue<string>();

        public int MaxDegreeOfParallelism
            => GetConfigurationValue<int>();

        public bool UseHttps
            => GetConfigurationValue<bool>();

        public int RetryDelayIntervalMin
            => GetConfigurationValue<int>();

        public int RetryCountPerInterval
            => GetConfigurationValue<int>();

		public int RecoveryIntervalMinutesMax
            => GetConfigurationValue<int>();

        public bool DisableRemoteRequests
            => GetConfigurationValue<bool>();

        public int NodeId => GetConfigurationValue<int>();
    }
}