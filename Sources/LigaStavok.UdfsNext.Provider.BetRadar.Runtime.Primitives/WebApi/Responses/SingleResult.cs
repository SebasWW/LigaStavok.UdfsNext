//namespace Udfs.BetradarUnifiedFeed.Api.Messages
//{
//    using System;
//    using System.ComponentModel;

//    using Common.Primitives;



//    [ImmutableObject(true)]
//    public partial class SingleResult : IHaveHeader
//    {
//        #region Header

//        /// <summary>
//        ///    Gets the incoming id of received messages.
//        /// </summary>
//        public Guid IncomingId { get; set; }

//        public Language Language { get; set; }

//        /// <summary>
//        ///    Gets the time, message was received on.
//        /// </summary>
//        public DateTimeOffset ReceivedOn { get; set; }

//        public Guid RequestId { get; set; }

//        public DateTimeOffset RequestedOn { get; set; }

//        #endregion

//        public CoverageInfo CoverageInfo { get; set; }

//        public DateTimeOffset? GeneratedOn { get; set; }

//        public SportEvent SportEvent { get; set; }

//        public SportEventConditions SportEventConditions { get; set; }

//        public SportEventStatus SportEventStatus { get; set; }



//        public static SingleResult Parse(dynamic dynamicXml, IHaveHeader headerSource)
//        {
//            if (dynamicXml == null)
//            {
//                return null;
//            }

//            var builder = new Builder
//            {
//                IncomingId           = headerSource.IncomingId,
//                Language             = headerSource.Language,
//                RequestId            = headerSource.RequestId,
//                RequestedOn          = headerSource.RequestedOn,
//                ReceivedOn           = headerSource.ReceivedOn,
//                CoverageInfo         = ApiMessages.CoverageInfo.Parse(dynamicXml.CoverageInfo),
//                GeneratedOn          = dynamicXml.GeneratedOn<DateTimeOffset?>(),
//                SportEvent           = ApiMessages.SportEvent.Parse(dynamicXml.SportEvent),
//                SportEventConditions = ApiMessages.SportEventConditions.Parse(dynamicXml.SportEventConditions),
//                SportEventStatus     = ApiMessages.SportEventStatus.Parse(dynamicXml.SportEventStatus),
//            };

//            return builder;
//        }
//    }
//}

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
}