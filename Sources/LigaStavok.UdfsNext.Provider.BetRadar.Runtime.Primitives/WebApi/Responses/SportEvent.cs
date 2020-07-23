using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportEvent
    {
        public List<Team> Competitors { get; set; }

        public string Id { get; set; }

        public string Liveodds { get; set; }

        public string Name { get; set; }

        public ParentStage Parent { get; set; }

        public IEnumerable<SportEventChild> Races { get; set; }

        public DateTimeOffset? Scheduled { get; set; }

        public DateTimeOffset? ScheduledEnd { get; set; }

        public Season Season { get; set; }

        public string Status { get; set; }

        public Tournament Tournament { get; set; }

        public MatchRound TournamentRound { get; set; }

        public string Type { get; set; }

        public static SportEvent Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportEvent
            {
                Competitors     = Team.ParseList(dynamicXml.Competitors?.GetCompetitorList()),
                Id              = dynamicXml.Id,
                Liveodds        = dynamicXml.Liveodds,
                Name            = dynamicXml.Name,
                Parent          = ParentStage.Parse(dynamicXml.Parent),
                Races           = SportEventChild.ParseList(dynamicXml.Races?.GetSportEventList()),
                Scheduled       = dynamicXml.Scheduled<DateTimeOffset?>(),
                ScheduledEnd    = dynamicXml.ScheduledEnd<DateTimeOffset?>(),
                Season          = Responses.Season.Parse(dynamicXml.Season),
                Status          = dynamicXml.Status,
                Tournament      = Responses.Tournament.Parse(dynamicXml.Tournament),
                TournamentRound = MatchRound.Parse(dynamicXml.TournamentRound),
                Type            = dynamicXml.Type
            };

            return builder;
        }

        public static IEnumerable<SportEvent> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<SportEvent>();
        }
    }
}