using System;
using System.Collections.Generic;
using System.Linq;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
	public partial class Fixture
    {
        public IEnumerable<Team> Competitors { get; set; }

        public CoverageInfo CoverageInfo { get; set; }

        public DelayedInfo DelayedInfo { get; set; }

        public Dictionary<string, string> ExtraInfo { get; set; }

        
        public string Id { get; set; }

        public string Liveodds { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? NextLiveTime { get; set; }

        public ParentStage Parent { get; set; }

        public ProductInfo ProductInfo { get; set; }

        public IEnumerable<SportEventChild> Races { get; set; }

        
        public Dictionary<string, string> ReferenceIds { get; set; }

        public DateTimeOffset? Scheduled { get; set; }

        public DateTimeOffset? ScheduledEnd { get; set; }

        public Season Season { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public bool? StartTimeConfirmed { get; set; }

        public string Status { get; set; }

        public Tournament Tournament { get; set; }

        public MatchRound TournamentRound { get; set; }

        public IEnumerable<TvChannel> TvChannels { get; set; }

        public string Type { get; set; }

        public Venue Venue { get; set; }



        public static Fixture Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Fixture
            {
                Competitors        = Team.ParseList(dynamicXml.Competitors?.GetCompetitorList()),
                CoverageInfo       = Responses.CoverageInfo.Parse(dynamicXml.CoverageInfo),
                DelayedInfo        = Responses.DelayedInfo.Parse(dynamicXml.DelayedInfo),
                ExtraInfo          = ParseExtraInfo(dynamicXml.ExtraInfo),
                Id                 = dynamicXml.Id,
                Liveodds           = dynamicXml.Liveodds,
                Name               = dynamicXml.Name,
                NextLiveTime       = dynamicXml.NextLiveTime<DateTimeOffset?>(),
                Parent             = ParentStage.Parse(dynamicXml.Parent),
                ProductInfo        = Responses.ProductInfo.Parse(dynamicXml.ProductInfo),
                Races              = SportEventChild.ParseList(dynamicXml.Races?.GetSportEventList()),
                ReferenceIds       = Responses.ReferenceIds.Parse(dynamicXml.ReferenceIds),
                Scheduled          = dynamicXml.Scheduled<DateTimeOffset?>(),
                ScheduledEnd       = dynamicXml.ScheduledEnd<DateTimeOffset?>(),
                Season             = Responses.Season.Parse(dynamicXml.Season),
                StartTime          = dynamicXml.StartTime<DateTimeOffset>(),
                StartTimeConfirmed = dynamicXml.StartTimeConfirmed<bool?>(),
                Status             = dynamicXml.Status,
                Tournament         = Responses.Tournament.Parse(dynamicXml.Tournament),
                TournamentRound    = MatchRound.Parse(dynamicXml.TournamentRound),
                TvChannels         = TvChannel.ParseList(dynamicXml.TvChannels?.GetTvChannelList()),
                Type               = dynamicXml.Type,
                Venue              = Responses.Venue.Parse(dynamicXml.Venue)
            };

            return builder;

            Dictionary<string, string> ParseExtraInfo(dynamic extraInfo)
            {
                return extraInfo == null
                     ? new Dictionary<string, string>()
                     : ((IEnumerable<dynamic>)extraInfo.GetInfoList()).ToDictionary(x => (string)x.Key, x => (string)x.Value);
            }
        }

        public static IEnumerable<Fixture> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<Fixture>();
        }
    }
}