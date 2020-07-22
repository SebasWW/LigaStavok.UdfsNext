using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TournamentList
    {
        public DateTimeOffset? GeneratedOn { get; set; }

        public IEnumerable<Tournament> Tournaments { get; set; }


        public static TournamentList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TournamentList
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                Tournaments = Tournament.ParseList(dynamicXml.GetTournamentList())
            };

            return builder;
        }
    }
}