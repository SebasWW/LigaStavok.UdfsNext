using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TournamentGroup
    {
        public IEnumerable<Team> Competitors { get; set; }

        public string Name { get; set; }

        public static TournamentGroup Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TournamentGroup
            {
                Competitors = Team.ParseList(dynamicXml.GetCompetitorList()),
                Name        = dynamicXml.Name
            };

            return builder;
        }


        public static IEnumerable<TournamentGroup> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<TournamentGroup>();
        }
    }
}