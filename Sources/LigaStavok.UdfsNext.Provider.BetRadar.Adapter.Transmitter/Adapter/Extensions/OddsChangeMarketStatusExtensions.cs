using System;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.Transmitter.Messages.Identifiers;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions
{
	public static class OddsChangeMarketStatusExtensions
    {
        public static TradingStatus ToMarketTradingStatus(this OddsChangeMarketStatus? marketStatus)
        {
            switch (marketStatus)
            {
                case OddsChangeMarketStatus.Suspended:
                case OddsChangeMarketStatus.HandedOver:
                    return TradingStatus.Suspended;
                case OddsChangeMarketStatus.Inactive:
                case OddsChangeMarketStatus.Settled:
                case OddsChangeMarketStatus.Cancelled:
                case null:
                    return TradingStatus.Closed;
                case OddsChangeMarketStatus.Active:
                    return TradingStatus.Open;
                default:
                    throw new ArgumentOutOfRangeException(nameof(marketStatus), marketStatus, null);
            }
        }
    }
}