using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using LigaStavok.UdfsNext.Provider.SportLevel.WebApi.Requests;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi
{
	public class HttpRequestMessageFactory : IHttpRequestMessageFactory
	{
		private readonly WebApiOptions options;

		public HttpRequestMessageFactory(
			IOptions<WebApiOptions> options
		)
		{
			this.options = options.Value;
		}

		public HttpRequestMessage Create(WebApiRequest requestCommand)
		{
			switch (requestCommand)
			{
				case SportsRequest msg:
					return HandleMessage(msg);
				case TournamentsRequest msg:
					return HandleMessage(msg);
				case TranslationsRequest msg:
					return HandleMessage(msg);
				case TranslationRequest msg:
					return HandleMessage(msg);
				default:
					throw new ArgumentOutOfRangeException(nameof(requestCommand));
			}
		}

		private HttpRequestMessage HandleMessage(SportsRequest request)
		{
			var relativeUri = "api/1.0/sports";
			var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

			message.Properties.Add(WebApiRequest.SourceKey, SportsRequest.SourceValue);

			return message;
		}

		private HttpRequestMessage HandleMessage(TournamentsRequest request)
		{
			var relativeUri = "api/1.0/tournaments";
			var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

			message.Properties.Add(WebApiRequest.SourceKey, TournamentsRequest.SourceValue);

			return message;
		}

		private HttpRequestMessage HandleMessage(TranslationsRequest request)
		{
			var relativeUri = $"api/1.0/translations?rnd={Guid.NewGuid()}"
					+ (request.FromISO8601.HasValue ? "&from_iso8601=" + request.FromISO8601.Value.ToString("yyyy-MM-dd") : string.Empty)
					+ (request.ToISO8601.HasValue ? "&to_iso8601=" + request.ToISO8601.Value.ToString("yyyyMMddHHmm") : string.Empty)
					+ (request.SportId.HasValue ? "&sport_id=" + request.SportId.Value.ToString() : string.Empty)
					+ (request.TournamentId.HasValue ? "&tournament_id=" + request.TournamentId.Value.ToString() : string.Empty)
					+ (!string.IsNullOrWhiteSpace(request.Booking) ? "&booking=" + request.Booking : string.Empty)
					+ (request.Length.HasValue ? "&length=" + request.Length.Value.ToString() : string.Empty)
					+ (request.Start.HasValue ? "&start=" + request.Start.Value.ToString() : string.Empty)
					+ (!string.IsNullOrWhiteSpace(request.Order_dir) ? "&order_dir=" + request.Order_dir : string.Empty)
					+ (request.StateIds?.Length > 0 ? string.Join(string.Empty, request.StateIds.Select(t => "&state_id[]=" + t)) : string.Empty);

			var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

			message.Properties.Add(WebApiRequest.SourceKey, TranslationsRequest.SourceValue);

			return message;
		}

		private HttpRequestMessage HandleMessage(TranslationRequest request)
		{
			var relativeUri = $"api/1.0/translations/{request.Id}";
			var message = new HttpRequestMessage(HttpMethod.Get, new Uri(options.Uri, relativeUri));

			message.Properties.Add(WebApiRequest.SourceKey, TranslationRequest.SourceValue);

			return message;
		}
	}
}

