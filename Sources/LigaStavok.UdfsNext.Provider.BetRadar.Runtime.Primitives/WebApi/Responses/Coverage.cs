using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Coverage
    {      
        public string Includes { get; set; }

        public static Coverage Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var coverageBuilder = new Coverage
            {
                Includes = dynamicXml.Includes
            };

            return coverageBuilder;
        }

        public static IEnumerable<Coverage> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Coverage>();
        }
    }
}