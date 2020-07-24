using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
{
	public class HttpRequestMessageFactory : IHttpRequestMessageFactory
	{
		private readonly HttpClientManagerOptions options;

		public HttpRequestMessageFactory(
			IOptions<HttpClientManagerOptions> options
		)
		{
			this.options = options.Value;
		}

		public HttpRequestMessage Create(IApiCommandCreateRequest requestCommand)
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

        private HttpRequestMessage HandleMessage(RequestBetstopReasons request)
        {
            var relativeUri = "descriptions/betstop_reasons.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestBettingStatuses request)
        {
            var relativeUri = "descriptions/betting_status.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestCompetitorProfile request)
        {
            var relativeUri = $"sports/{request.Language.Code}/competitors/{request.CompetitorId}/profile.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestEventSummary request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sport_events/{request.EventId}/summary.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestEventTimeline request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sport_events/{request.EventId}/timeline.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestFixtureChanges request)
        {
            var relativeUri = $"sports/{request.Language.Code}/fixtures/changes.xml"
                + (request.AfterDateTime.HasValue ? $"?afterDateTime={request.AfterDateTime.Value:yyyy-MM-ddTHH:mm:ss}" : string.Empty);
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestFixture request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sport_events/{request.EventId}/fixture.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestLiveSchedule request)
        {
            var relativeUri = $"sports/{request.Language.Code}/schedules/live/schedule.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestMarketDescriptions request)
        {
            var relativeUri = request.IncludeMappins
                    ? $"descriptions/{request.Language.Code}/markets.xml?include_mappings=true"
                    : $"descriptions/{request.Language.Code}/markets.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestMatchStatuses request)
        {
            var relativeUri = $"descriptions/{request.Language.Code}/match_status.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestOddsRecovery request)
        {
            if (request.EventId == null && !request.FullRecovery.HasValue && !request.RecoverAfter.HasValue)
            {
                var relativeUri = $"{GetProductCode(request.ProductType)}/recovery/initiate_request?request_id={request.RequestId}";
                var message = new HttpRequestMessage(HttpMethod.Post, new Uri(options.Uri, relativeUri));

                return message;
            }

            if (request.EventId != null && request.FullRecovery.HasValue && !request.RecoverAfter.HasValue)
            {
                var relativeUri = $"{GetProductCode(request.ProductType)}/{(request.FullRecovery.Value ? "odds" : "stateful_messages")}/events/{request.EventId}/initiate_request?request_id={request.RequestId}";
                var message = new HttpRequestMessage(HttpMethod.Post, new Uri(options.Uri, relativeUri));

                return message;
            }

            if (request.EventId == null && !request.FullRecovery.HasValue && request.RecoverAfter.HasValue)
            {
                // checking for max recovery interval
                var ts = DateTimeOffset.FromUnixTimeMilliseconds(request.RecoverAfter.Value);
                //if (DateTimeOffset.UtcNow.Subtract(ts).TotalMinutes > _apiConfiguration.RecoveryIntervalMinutesMax)
                //{
                //    ts = DateTimeOffset.UtcNow.AddMinutes(-_apiConfiguration.RecoveryIntervalMinutesMax);
                //}

                var relativeUri = $"{GetProductCode(request.ProductType)}/recovery/initiate_request?after={ts.ToUnixTimeMilliseconds()}&request_id={request.RequestId}";
                var message = new HttpRequestMessage(HttpMethod.Post, new Uri(options.Uri, relativeUri));

                return message;
            }

            throw new ArgumentException($"{nameof(RequestOddsRecovery)} command is incorrect.");
        }

        private HttpRequestMessage HandleMessage(RequestOutcomeReasons request)
        {
            var relativeUri = "descriptions/outcome_reasons.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestPlayerProfileRequest request)
        {
            var relativeUri = $"sports/{request.Language.Code}/players/{request.PlayerId}/profile.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }

        private HttpRequestMessage HandleMessage(RequestSchedule request)
        {
            var relativeUri = $"sports/{request.Language.Code}/schedules/{request.Date:yyyy-MM-dd}/schedule.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestCreateSportCategories request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sports/{request.SportId}/categories.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestSports request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sports.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestSportTournaments request)
        {
            var relativeUri = $"sports/{request.Language.Code}/sports/{request.SportId}/tournaments.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestTournamentInfo request)
        {
            var relativeUri = $"sports/{request.Language.Code}/tournaments/{request.TournamentId}/info.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestTournamentSchedule request)
        {
            var relativeUri = $"sports/{request.Language.Code}/tournaments/{request.TournamentId}/schedule.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestTournaments request)
        {
            var relativeUri = $"sports/{request.Language.Code}/tournaments.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestVariantMarketDescription request)
        {
            var relativeUri = $"descriptions/{request.Language.Code}/markets/{request.MarketId}/variants/{request.Variant}";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestDirectVariantMarketDescription request)
        {
            var relativeUri = $"{GetProductCode(request.ProductType)}/descriptions/{request.Language.Code}/markets/{request.MarketId}/variants/{request.Variant}";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestVenueSummary request)
        {
            var relativeUri = $"sports/{request.Language.Code}/venues/{request.VenueId}/summary.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
        }


        private HttpRequestMessage HandleMessage(RequestVoidReasonsRequest request)
        {
            var relativeUri = "descriptions/void_reasons.xml";
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

            return message;
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

