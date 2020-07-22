using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportTournamentList
    {
        public DateTimeOffset? GeneratedOn { get; set; }

        public Sport Sport { get; set; }

        public IEnumerable<Tournament> Tournaments { get; set; }

        public static SportTournamentList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportTournamentList
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset>(),
                Sport       = Responses.Sport.Parse(dynamicXml.Sport),
                Tournaments = Tournament.ParseList(dynamicXml.Tournaments?.GetTournamentList())
            };

            return builder;
        }
    }
}