using System;
using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class TournamentInfo
    {        
        public Tournament Tournament { get; set; }

        public Season Season { get; set; }

        public MatchRound Round { get; set; }

        public SeasonCoverageInfo SeasonCoverageInfo { get; set; }

        public IEnumerable<TournamentGroup> Groups { get; set; }

        public IEnumerable<Team> Competitors { get; set; }

        public IEnumerable<Tournament> Children { get; set; }
        
        public CoverageInfo CoverageInfo { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }
        
        public static TournamentInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TournamentInfo
            {
                Tournament         = Responses.Tournament.Parse(dynamicXml.Tournament),
                Season             = Responses.Season.Parse(dynamicXml.Season),
                Round              = MatchRound.Parse(dynamicXml.Round),
                SeasonCoverageInfo = Responses.SeasonCoverageInfo.Parse(dynamicXml.SeasonCoverageInfo),
                Groups             = TournamentGroup.ParseList(dynamicXml.Groups?.GetGroupList()),
                Competitors        = Team.ParseList(dynamicXml.Competitors?.GetCompetitorList()),
                Children           = Responses.Tournament.ParseList(dynamicXml.Children?.GetTournamentList()),
                CoverageInfo       = Responses.CoverageInfo.Parse(dynamicXml.CoverageInfo),
                GeneratedOn        = dynamicXml.GeneratedAt<DateTimeOffset?>()
            };

            return builder;
        }
    }
}