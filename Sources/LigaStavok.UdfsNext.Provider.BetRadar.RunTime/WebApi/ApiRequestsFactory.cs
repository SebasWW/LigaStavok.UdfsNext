using System;
using System.Net.Http;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;
using Udfs.BetradarUnifiedFeed.Plugin.Adapter.Extensions;
using System.Collections.Generic;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
    public sealed class ApiRequestsFactory : IApiRequestsFactory
    {
        private readonly Uri _baseUri;
        private readonly ApiConfiguration _apiConfiguration;

        public ApiRequestsFactory(ApiConfiguration configuration)
        {
            _baseUri = new Uri($"{(configuration.UseHttps ? "https://" : "http://")}{configuration.Host}/v1/");
            _apiConfiguration = configuration;
        }

        private ApiCommand CreateApiRequestCreatedEvent(

            HttpMethod method,
            string relativeUri,
            TimeSpan cachePeriod,
            LineService lineService,
            Language language = null,
            bool needRetry = false,
            string eventId = ""
        )
        {
            var customHeaders = new Dictionary<string, string>();

            customHeaders.Add("x-access-token", _apiConfiguration.Key);

            return new ApiCommand()
            {
                CachePeriod = cachePeriod,
                CustomHeaders = customHeaders,
                Endpoint = new Uri(_baseUri, relativeUri),
                GeneratedOn = DateTimeOffset.UtcNow,
                RequestId = Guid.NewGuid(),
                Language = language,
                Method = method,
                IsRecovery = needRetry,
                EventId = eventId,
                LineService = lineService
            };
        }

        public ApiCommand CreateRequestMessage(IApiCommandCreateRequest requestCommand)
        {
            switch (requestCommand)
            {
                case RequestBetstopReasons msg:
                    return HandleMessage(msg);
                case RequestBettingStatuses msg:
                    return HandleMessage(msg);
                case RequestCompetitorProfile msg:
                    return HandleMessage(msg);
                case RequestCreateSportCategories msg:
                    return HandleMessage(msg);
                case RequestDirectVariantMarketDescription msg:
                    return HandleMessage(msg);
                case RequestEventSummary msg:
                    return HandleMessage(msg);
                case RequestEventTimeline msg:
                    return HandleMessage(msg);
                case RequestFixture msg:
                    return HandleMessage(msg);
                case RequestFixtureChanges msg:
                    return HandleMessage(msg);
                case RequestLiveSchedule msg:
                    return HandleMessage(msg);
                case RequestMarketDescriptions msg:
                    return HandleMessage(msg);
                case RequestMatchStatuses msg:
                    return HandleMessage(msg);
                case RequestOddsRecovery msg:
                    return HandleMessage(msg);
                case RequestOutcomeReasons msg:
                    return HandleMessage(msg);
                case RequestPlayerProfileRequest msg:
                    return HandleMessage(msg);
                case RequestSchedule msg:
                    return HandleMessage(msg);
                case RequestSports msg:
                    return HandleMessage(msg);
                case RequestSportTournaments msg:
                    return HandleMessage(msg);
                case RequestTournamentInfo msg:
                    return HandleMessage(msg);
                case RequestTournaments msg:
                    return HandleMessage(msg);
                case RequestTournamentSchedule msg:
                    return HandleMessage(msg);
                case RequestVariantMarketDescription msg:
                    return HandleMessage(msg);
                case RequestVenueSummary msg:
                    return HandleMessage(msg);
                case RequestVoidReasonsRequest msg:
                    return HandleMessage(msg);
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestCommand));
            }
        }

        private ApiCommand HandleMessage(RequestBetstopReasons message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: "descriptions/betstop_reasons.xml",
                cachePeriod: TimeSpan.FromDays(1),
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestBettingStatuses message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: "descriptions/betting_status.xml",
                cachePeriod: TimeSpan.FromDays(1),
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestCompetitorProfile message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/competitors/{message.CompetitorId}/profile.xml",
                cachePeriod: TimeSpan.FromMinutes(10),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestEventSummary message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sport_events/{message.EventId}/summary.xml",
                cachePeriod: TimeSpan.Zero,
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestEventTimeline message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sport_events/{message.EventId}/timeline.xml",
                cachePeriod: TimeSpan.Zero,
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestFixtureChanges message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/fixtures/changes.xml",
                cachePeriod: TimeSpan.Zero,
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestFixture message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sport_events/{message.EventId}/fixture.xml",
                cachePeriod: TimeSpan.Zero,
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestLiveSchedule message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/schedules/live/schedule.xml",
                cachePeriod: TimeSpan.Zero,
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestMarketDescriptions message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: message.IncludeMappins
                    ? $"descriptions/{message.Language.Code}/markets.xml?include_mappings=true"
                    : $"descriptions/{message.Language.Code}/markets.xml",
                cachePeriod: TimeSpan.FromDays(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestMatchStatuses message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"descriptions/{message.Language.Code}/match_status.xml",
                cachePeriod: TimeSpan.FromDays(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        private ApiCommand HandleMessage(RequestOddsRecovery message)
        {
            if (message.EventId == null && !message.FullRecovery.HasValue && !message.RecoverAfter.HasValue)
            {
                return CreateApiRequestCreatedEvent(
                    method: HttpMethod.Post,
                    relativeUri: $"{GetProductCode(message.ProductType)}/recovery/initiate_request?request_id={message.RequestId}&node_id={_apiConfiguration.NodeId}",
                    cachePeriod: TimeSpan.Zero,
                    eventId: message.EventId,
                    lineService: message.ProductType.ToLineService()
                );
            }

            if (message.EventId != null && message.FullRecovery.HasValue && !message.RecoverAfter.HasValue)
            {
                return CreateApiRequestCreatedEvent(
                    method: HttpMethod.Post,
                    relativeUri: $"{GetProductCode(message.ProductType)}/{(message.FullRecovery.Value ? "odds" : "stateful_messages")}/events/{message.EventId}/initiate_request?request_id={message.RequestId}&node_id={_apiConfiguration.NodeId}",
                    cachePeriod: TimeSpan.Zero,
                    eventId: message.EventId,
                    lineService: message.ProductType.ToLineService()
                );
            }

            if (message.EventId == null && !message.FullRecovery.HasValue && message.RecoverAfter.HasValue)
            {
                // checking for max recovery interval
                var ts = DateTimeOffset.FromUnixTimeMilliseconds(message.RecoverAfter.Value);
                if (DateTimeOffset.UtcNow.Subtract(ts).TotalMinutes > _apiConfiguration.RecoveryIntervalMinutesMax)
                {
                    ts = DateTimeOffset.UtcNow.AddMinutes(-_apiConfiguration.RecoveryIntervalMinutesMax);
                }

                return CreateApiRequestCreatedEvent(
                    method: HttpMethod.Post,
                    relativeUri: $"{GetProductCode(message.ProductType)}/recovery/initiate_request?after={ts.ToUnixTimeMilliseconds()}&request_id={message.RequestId}&node_id={_apiConfiguration.NodeId}",
                    cachePeriod: TimeSpan.Zero,
                    needRetry: true,
                    eventId: message.EventId,
                    lineService: message.ProductType.ToLineService()
                );
            }

            throw new ArgumentException($"{nameof(RequestOddsRecovery)} command is incorrect.");
        }

        private ApiCommand HandleMessage(RequestOutcomeReasons message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: "descriptions/outcome_reasons.xml",
                cachePeriod: TimeSpan.FromDays(1),
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestPlayerProfileRequest message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/players/{message.PlayerId}/profile.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestSchedule message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/schedules/{message.Date:yyyy-MM-dd}/schedule.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestCreateSportCategories message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sports/{message.SportId}/categories.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestSports message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sports.xml",
                cachePeriod: TimeSpan.FromDays(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestSportTournaments message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/sports/{message.SportId}/tournaments.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestTournamentInfo message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/tournaments/{message.TournamentId}/info.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestTournamentSchedule message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/tournaments/{message.TournamentId}/schedule.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestTournaments message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/tournaments.xml",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestVariantMarketDescription message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"descriptions/{message.Language.Code}/markets/{message.MarketId}/variants/{message.Variant}",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestDirectVariantMarketDescription message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"{GetProductCode(message.ProductType)}/descriptions/{message.Language.Code}/markets/{message.MarketId}/variants/{message.Variant}",
                cachePeriod: TimeSpan.FromHours(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestVenueSummary message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: $"sports/{message.Language.Code}/venues/{message.VenueId}/summary.xml",
                cachePeriod: TimeSpan.FromDays(1),
                language: message.Language,
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }

        
        private ApiCommand HandleMessage(RequestVoidReasonsRequest message)
        {
            return CreateApiRequestCreatedEvent(
                method: HttpMethod.Get,
                relativeUri: "descriptions/void_reasons.xml",
                cachePeriod: TimeSpan.FromDays(1),
                eventId: message.EventId,
                lineService: message.ProductType.ToLineService()
            );
        }


        
        private static string GetProductCode(ProductType product)
        {
            switch (product)
            {
                case ProductType.LiveOdds:
                    return "liveodds";
                case ProductType.LCoO:
                    return "pre";
                case ProductType.BetPal:
                    return "betpal";
                case ProductType.MTS:
                case ProductType.PremiumCricket:
                case ProductType.Virtuals:
                    throw new NotSupportedException($"Requests for specified product ['{product}'] are not supported.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(product), product, null);
            }
        }
    }
}