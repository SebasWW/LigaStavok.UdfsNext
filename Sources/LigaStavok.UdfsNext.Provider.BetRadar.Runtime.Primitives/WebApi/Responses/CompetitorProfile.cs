using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class CompetitorProfile
    {
       
        public Team Competitor { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }

        public IEnumerable<Jersey> Jerseys { get; set; }

        public Manager Manager { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public RaceDriverProfile RaceDriverProfile { get; set; }

        public Venue Venue { get; set; }

        public static CompetitorProfile Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }
            
            var builder = new CompetitorProfile
            {
                Competitor        = Team.Parse(dynamicXml.Competitor),
                GeneratedOn       = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                Jerseys           = Jersey.ParseList(dynamicXml.Jerseys?.GetJerseyList()),
                Manager           = Responses.Manager.Parse(dynamicXml.Manager),
                Players           = Player.ParseList(dynamicXml.Players?.GetPlayerList()),
                RaceDriverProfile = Responses.RaceDriverProfile.Parse(dynamicXml.RaceDriverProfile),
                Venue             = Responses.Venue.Parse(dynamicXml.Venue)
            };

            return builder;
        }
    }
}