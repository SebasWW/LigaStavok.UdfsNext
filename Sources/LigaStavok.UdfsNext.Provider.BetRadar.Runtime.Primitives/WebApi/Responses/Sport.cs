using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Sport
    {       
        public string Id { get; set; }

        public string Name { get; set; }

        public static Sport Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Sport
            {
                Id   = dynamicXml.Id,
                Name = dynamicXml.Name
            };

            return builder;
        }

        public static IEnumerable<Sport> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Sport>();
        }
    }


}