using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class MarketDescriptionList
    {
        public IEnumerable<MarketDescription> Markets { get; set; }

        public static MarketDescriptionList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new MarketDescriptionList
            {
                Markets = MarketDescription.ParseList(dynamicXml.GetMarketList())
            };

            return builder;
        }
    }
}