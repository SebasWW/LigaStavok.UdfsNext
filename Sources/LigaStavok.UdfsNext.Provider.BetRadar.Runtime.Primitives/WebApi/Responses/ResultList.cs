//namespace Udfs.BetradarUnifiedFeed.Api.Messages
//{
//    using System;
//    using System;
//    using System.ComponentModel;

//    using Common.Primitives;

//    



//    [ImmutableObject(true)]
//    public partial class ResultList : IHaveHeader
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

//        public DateTimeOffset? GeneratedOn { get; set; }

//        public IEnumerable<Result> Results { get; set; }



//        
//        public static ResultList Parse(dynamic dynamicXml, IHaveHeader headerSource)
//        {
//            if (dynamicXml == null)
//            {
//                return null;
//            }

//            var builder = new Builder
//            {
//                IncomingId  = headerSource.IncomingId,
//                Language    = headerSource.Language,
//                RequestId   = headerSource.RequestId,
//                RequestedOn = headerSource.RequestedOn,
//                ReceivedOn  = headerSource.ReceivedOn,
//                GeneratedOn = dynamicXml.GeneratedOn<DateTimeOffset>(),
//                Results     = Result.ParseList(dynamicXml.GetResultList())
//            };

//            return builder;
//        }
//    }
//}

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
}