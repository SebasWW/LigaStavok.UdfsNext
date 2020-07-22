using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class ProductInfo
    {
        public bool IsAutoTraded { get; set; }

        public bool IsInHostedStatistics { get; set; }

        public bool IsInLiveCenterSoccer { get; set; }

        public bool IsInLiveScore { get; set; }

        public IEnumerable<ProductInfoLink> Links { get; set; }

        public IEnumerable<StreamingChannel> Streamings { get; set; }

        public static ProductInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new ProductInfo
            {
                IsAutoTraded         = dynamicXml.IsAutoTraded != null,
                IsInHostedStatistics = dynamicXml.IsInHostedStatistics != null,
                IsInLiveCenterSoccer = dynamicXml.IsInLiveCenterSoccer != null,
                IsInLiveScore        = dynamicXml.IsInLiveScore != null,
                Links                = ProductInfoLink.ParseList(dynamicXml.Links?.GetLinkList()),
                Streamings           = StreamingChannel.ParseList(dynamicXml.Streaming?.GetChannelList())
            };

            return builder;

        }
    }
}