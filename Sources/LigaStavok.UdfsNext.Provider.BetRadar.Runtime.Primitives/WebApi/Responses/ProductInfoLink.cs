using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class ProductInfoLink
    {        
        public string Name { get; set; }

        public string Ref { get; set; }

        public static ProductInfoLink Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new ProductInfoLink
            {
                Name = dynamicXml.Name,
                Ref  = dynamicXml.Ref
            };

            return builder;
        }
        
        public static IEnumerable<ProductInfoLink> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<ProductInfoLink>();
        }
    }
}