using System;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Tournament
    {
        public Category Category { get; set; }

        public IEnumerable<Team> Competitors { get; set; }

        public Season CurrentSeason { get; set; }

        public string Id { get; set; }
        
        public string Name { get; set; }

        public DateTimeOffset? Scheduled { get; set; }

        public DateTimeOffset? ScheduledEnd { get; set; }

        public SeasonCoverageInfo SeasonCoverageInfo { get; set; }

        public Sport Sport { get; set; }

        public TournamentLength TournamentLength { get; set; }

        public static Tournament Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Tournament
            {
                Category           = Responses.Category.Parse(dynamicXml.Category),
                Competitors        = Team.ParseList(dynamicXml.Competitors?.GetCompetitorList()),
                CurrentSeason      = Season.Parse(dynamicXml.CurrentSeason),
                Id                 = dynamicXml.Id,
                Name               = dynamicXml.Name,
                Scheduled          = dynamicXml.Scheduled<DateTimeOffset?>(),
                ScheduledEnd       = dynamicXml.ScheduledEnd<DateTimeOffset?>(),
                SeasonCoverageInfo = Responses.SeasonCoverageInfo.Parse(dynamicXml.SeasonCoverageInfo),
                Sport              = Responses.Sport.Parse(dynamicXml.Sport),
                TournamentLength   = Responses.TournamentLength.Parse(dynamicXml.TournamentLength)
            };

            return builder;
        }

        public static IEnumerable<Tournament> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Tournament>();
        }
    }
}