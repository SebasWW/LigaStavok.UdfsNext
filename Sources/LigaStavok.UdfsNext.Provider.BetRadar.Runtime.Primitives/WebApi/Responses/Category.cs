using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Category
    {
        public string CountryCode { get; set; }

        public string Id { get; set; }
        
        public string Name { get; set; }

        public static Category Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Category
            {
                CountryCode = dynamicXml.CountryCode,
                Id          = dynamicXml.Id,
                Name        = dynamicXml.Name,
            };

            return builder;
        }

        public static IEnumerable<Category> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Category>();
        }
    }
}