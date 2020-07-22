using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class PeriodScore
    {
        public double HomeScore { get; set; }

        public double AwayScore { get; set; }

        public string Type { get; set; }

        public int? Number { get; set; }

        public static PeriodScore Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new PeriodScore
            {
                HomeScore = dynamicXml.HomeScore<double>(),
                AwayScore = dynamicXml.AwayScore<double>(),
                Type      = dynamicXml.Type<string>(),
                Number    = dynamicXml.Number<int?>(),
            };

            return builder;
        }

        public static IEnumerable<PeriodScore> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                   ?? Array.Empty<PeriodScore>();
        }
    }
}