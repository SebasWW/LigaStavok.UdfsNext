//namespace Udfs.BetradarUnifiedFeed.Api.Messages
//{
//    using System.Collections.Generic;
//    using System;
//    using System.ComponentModel;
//    using System.Linq;


//    [ImmutableObject(true)]
//    public partial class Result
//    {
//        public IEnumerable<SportEvent> SportEvents { get; set; }

//        public IEnumerable<SportEventStatus> SportEventStatuses { get; set; }



//        public static Result Parse(dynamic dynamicXml)
//        {
//            if (dynamicXml == null)
//            {
//                return null;
//            }

//            var builder = new Builder
//            {
//                SportEvents        = SportEvent.ParseList(dynamicXml.GetSportEventList()),
//                SportEventStatuses = SportEventStatus.ParseList(dynamicXml.GetSportEventStatusList)
//            };

//            return builder;
//        }

//        public static IEnumerable<Result> ParseList(IEnumerable<dynamic> dynamicList)
//        {
//            return dynamicList?.Select(Parse).ToArray()
//                   ?? IEnumerable<Result>.Empty;
//        }
//    }
//}

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
}