//using System;
//using NLog;
//using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
//using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor.Messages;
//using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
//using Udfs.Common.Helpers;
//using Udfs.Common.Messages;

//namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
//{
//    public sealed class FeedMessageParser : IFeedMessageParser
//    {
//        private readonly ILogger _logger;

//        public FeedMessageParser(LogFactory logFactory)
//        {
//            _logger = logFactory.GetCurrentClassLogger();
//        }

//        public IFeedMessageParsingResult ParseFeedMessageText(IHaveHeader sourceMessage, string decodedText)
//        {

//            var feedMessageType = FeedMessageType.Unknown;

//            try
//            {
//                IFeedMessage feedMessage;

//                var feedMessageXml = DynamicXml.Parse(decodedText);

//                feedMessageType = feedMessageXml.GetName();

//                switch (feedMessageType)
//                {
//                    case FeedMessageType.Alive:
//                        feedMessage = Alive.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.BetCancel:
//                        feedMessage = BetCancel.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.BetSettlement:
//                        feedMessage = BetSettlement.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.BetStop:
//                        feedMessage = BetStop.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.FixtureChange:
//                        feedMessage = FixtureChange.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.OddsChange:
//                        feedMessage = OddsChange.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.ProductDown:
//                        feedMessage = ProductDown.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.RollbackBetSettlement:
//                        feedMessage = BetSettlementRollback.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.RollbackBetCancel:
//                        feedMessage = BetCancelRollback.Parse(feedMessageXml);
//                        break;
//                    case FeedMessageType.SnapshotComplete:
//                        feedMessage = SnapshotComplete.Parse(feedMessageXml);
//                        break;
//                    default:
//                        throw new NotSupportedException($"Message of specified type ['{feedMessageType}'] can not be parsed.");
//                }

//                return new FeedMessageParsed(
//                    incomingId: sourceMessage.IncomingId,
//                    receivedOn: sourceMessage.ReceivedOn,
//                    message: feedMessage
//                );
//            }
//            catch (Exception e)
//            {
//                _logger.Error(e, $"ParseFeedMessageText for message type='{feedMessageType}'");

//                return new FeedMessageParsingFailed(
//                    incomingId: sourceMessage.IncomingId,
//                    receivedOn: sourceMessage.ReceivedOn,
//                    failureReason: e,
//                    messageData: decodedText,
//                    messageType: feedMessageType
//                );
//            }
//        }
//    }
//}