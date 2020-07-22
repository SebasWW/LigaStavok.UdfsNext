using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class VenueSummary
    {
        public DateTimeOffset? GeneratedOn { get; set; }

        public IEnumerable<Team> HomeTeams { get; set; }

        public Venue Venue { get; set; }
        
        public static VenueSummary Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new VenueSummary
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                HomeTeams   = Team.ParseList(dynamicXml.HomeTeams?.GetCompetitorList()),
                Venue       = Responses.Venue.Parse(dynamicXml.Venue),
            };

            return builder;
        }
    }
}