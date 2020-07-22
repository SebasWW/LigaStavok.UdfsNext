//namespace Udfs.BetradarUnifiedFeed.Api.Messages
//{
//    using System.Collections.Generic;
//    using System;
//    using System.ComponentModel;
//    using System.Linq;

//    



//    [ImmutableObject(true)]
//    public partial class StageResultCompetitor
//    {
//        public string Climber { get; set; }

//        public string ClimberRanking { get; set; }

//        public string Id { get; set; }

//        public string Points { get; set; }

//        public string Position { get; set; }

//        public string Sprint { get; set; }

//        public string SprintRanking { get; set; }

//        public string Status { get; set; }

//        public string StatusComment { get; set; }

//        public string Time { get; set; }

//        public string TimeRanking { get; set; }



//        
//        public static StageResultCompetitor Parse(dynamic dynamicXml)
//        {
//            if (dynamicXml == null)
//            {
//                return null;
//            }

//            var builder = new Builder
//            {
//                Climber        = dynamicXml.Climber,
//                ClimberRanking = dynamicXml.ClimberRanking,
//                Id             = dynamicXml.Id,
//                Points         = dynamicXml.Points,
//                Position       = dynamicXml.Position,
//                Sprint         = dynamicXml.Sprint,
//                SprintRanking  = dynamicXml.SprintRanking,
//                Status         = dynamicXml.Status,
//                StatusComment  = dynamicXml.StatusComment,
//                Time           = dynamicXml.Time,
//                TimeRanking    = dynamicXml.TimeRanking,
//            };

//            return builder;
//        }


//        public static IEnumerable<StageResultCompetitor> ParseList(IEnumerable<dynamic> dynamicList)
//        {
//            return dynamicList?.Select(Parse).ToArray()
//                ?? IEnumerable<StageResultCompetitor>.Empty;
//        }
//    }
//}

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
}