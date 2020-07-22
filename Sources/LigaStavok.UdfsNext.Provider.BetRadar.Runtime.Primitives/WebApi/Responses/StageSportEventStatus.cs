//namespace Udfs.BetradarUnifiedFeed.Api.Messages
//{
//    using System;
//    using System.ComponentModel;




//    [ImmutableObject(true)]
//    public partial class StageSportEventStatus
//    {
//        public IEnumerable<StageResultCompetitor> Results { get; set; }

//        public string Status { get; set; }

//        public string WinnerId { get; set; }



//        public static StageSportEventStatus Parse(dynamic dynamicXml)
//        {
//            if (dynamicXml == null)
//            {
//                return null;
//            }

//            var builder = new Builder
//            {
//                Results  = StageResultCompetitor.ParseList(dynamicXml.GetStageResultCompetitorList()),
//                Status   = dynamicXml.Status,
//                WinnerId = dynamicXml.WinnerId
//            };

//            return builder;
//        }
//    }
//}

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
}