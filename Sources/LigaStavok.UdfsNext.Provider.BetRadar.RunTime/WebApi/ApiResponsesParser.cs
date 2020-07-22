﻿using System;
using JetBrains.Annotations;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using Udfs.BetradarUnifiedFeed.Plugin.Adapter.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses;
using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor.Messages;
using Udfs.Common.Helpers;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
    public class ApiResponsesParser : IApiResponsesParser
    {
        
        public IApiResponseParsingResult ParseResponse(ParseApiResponse message)
        {
            var responseType = ApiResponseType.Unknown;

            try
            {
                object response;

                var responseXml = DynamicXml.Parse(message.Data);

                responseType = responseXml.GetName();

                switch (responseType)
                {
                    case ApiResponseType.CompetitorProfile:
                        response = CompetitorProfile.Parse(responseXml);
                        break;
                    case ApiResponseType.FixtureChanges:
                        response = FixtureChangeList.Parse(responseXml);
                        break;
                    case ApiResponseType.FixturesFixture:
                        response = FixtureList.Parse(responseXml);
                        break;
                    case ApiResponseType.MatchSummary:
                        response = MatchSummary.Parse(responseXml);
                        break;
                    case ApiResponseType.MarketDescriptions:
                        response = MarketDescriptionList.Parse(responseXml);
                        break;
                    case ApiResponseType.MatchTimeline:
                        response = MatchTimeline.Parse(responseXml);
                        break;
                    case ApiResponseType.PlayerProfile:
                        response = PlayerProfile.Parse(responseXml);
                        break;
                    case ApiResponseType.RaceTournamentInfo:
                    case ApiResponseType.SimpleTournamentInfo:
                    case ApiResponseType.StandardTournamentInfo:
                    case ApiResponseType.TournamentInfo:
                        response = TournamentInfo.Parse(responseXml);
                        break;
                    case ApiResponseType.Response:
                        response = Response.Parse(responseXml);
                        break;
                    case ApiResponseType.Schedule:
                        response = Schedule.Parse(responseXml);
                        break;
                    case ApiResponseType.SportCategories:
                        response = SportCategoriesList.Parse(responseXml);
                        break;
                    case ApiResponseType.Sports:
                        response = SportList.Parse(responseXml);
                        break;
                    case ApiResponseType.SportTournaments:
                        response = SportTournamentList.Parse(responseXml);
                        break;
                    case ApiResponseType.Tournaments:
                        response = TournamentList.Parse(responseXml);
                        break;
                    case ApiResponseType.VenueSummary:
                        response = VenueSummary.Parse(responseXml);
                        break;
                    default:
                        throw new NotSupportedException(
                            $"Message of specified type ['{responseType}'] can not be parsed.");
                }

                return new ApiResponseParsed(
                    incomingId: message.IncomingId,
                    receivedOn: message.ReceivedOn,
                    language: message.Language,
                    response: response,
                    requestId: message.RequestId,
					eventId: message.EventId,
					lineService: message.LineService
                );
            }
            catch (Exception e)
            {
                return new ApiResponseParsingFailed(
                    incomingId: message.IncomingId,
                    receivedOn: message.ReceivedOn,
                    failureReason: e,
                    requestId: message.RequestId,
                    responseData: message.Data,
                    responseType: responseType
                );
            }
        }
    }
}