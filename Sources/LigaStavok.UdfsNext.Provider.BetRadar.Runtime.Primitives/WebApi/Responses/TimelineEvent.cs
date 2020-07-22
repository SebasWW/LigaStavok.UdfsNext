using System;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TimelineEvent
    {
        public IEnumerable<TimelineEventPlayer> Assists { get; set; }

        public double? AwayScore { get; set; }

        public TimelineEventPlayer GoalScorer { get; set; }

        public double? HomeScore { get; set; }

        public int Id { get; set; }

        public int? MatchTime { get; set; }

        public string Period { get; set; }

        public string PeriodName { get; set; }

        public string Points { get; set; }

        public string StoppageTime { get; set; }

        public string Team { get; set; }

        public DateTimeOffset Time { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public int? X { get; set; }

        public int? Y { get; set; }
        
        public static TimelineEvent Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TimelineEvent
            {
                Assists      = TimelineEventPlayer.ParseList(dynamicXml.GetAssistList()),
                AwayScore    = dynamicXml.AwayScore<double?>(),
                HomeScore    = dynamicXml.HomeScore<double?>(),
                GoalScorer   = TimelineEventPlayer.Parse(dynamicXml.GoalScorer),
                Id           = dynamicXml.Id<int>(),
                MatchTime    = dynamicXml.MatchTime<int?>(),
                Period       = dynamicXml.Period,
                PeriodName   = dynamicXml.PeriodName,
                Points       = dynamicXml.Points,
                StoppageTime = dynamicXml.StoppageTime,
                Team         = dynamicXml.Team,
                Time         = dynamicXml.Time<DateTimeOffset>(),
                Type         = dynamicXml.Type,
                Value        = dynamicXml.Value,
                X            = dynamicXml.X<int?>(),
                Y            = dynamicXml.Y<int?>()
            };

            return builder;
        }

        public static IEnumerable<TimelineEvent> ParseList(IEnumerable<dynamic> dynamicList)
        {
            return dynamicList?.Select(Parse).ToArray()
                ?? Array.Empty<TimelineEvent>();
        }
    }
}