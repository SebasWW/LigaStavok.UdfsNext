using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Jersey
    {
        
        public string Type { get; set; }

        
        public string Base { get; set; }

        
        public string Sleeve { get; set; }

        
        public string Number { get; set; }

        public bool? Stripes { get; set; }

        public bool? HorizontalStripes { get; set; }

        public bool? SquaresField { get; set; }

        public bool? SplitField { get; set; }



        
        public static Jersey Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Jersey
            {
                Type              = dynamicXml.Type,
                Base              = dynamicXml.Base,
                Sleeve            = dynamicXml.Sleeve,
                Number            = dynamicXml.Number,
                Stripes           = dynamicXml.Stripes<bool?>(),
                HorizontalStripes = dynamicXml.HorizontalStripes<bool?>(),
                SquaresField      = dynamicXml.Squares<bool?>(),
                SplitField        = dynamicXml.Split<bool?>(),
            };

            return builder;
        }


        public static IEnumerable<Jersey> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Jersey>();
        }
    }
}