//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Akka.Actor;
//using NLog;
//using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
//using Udfs.BetradarUnifiedFeed.Plugin.Adapter;
//using Udfs.BetradarUnifiedFeed.Plugin.Adapter.Messages;
//using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Messages;
//using LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Requests;
//using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor;
//using Udfs.BetradarUnifiedFeed.Plugin.FailureSupervisor.Messages;
//using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ;
//using Udfs.BetradarUnifiedFeed.Plugin.InputDumping;
//using Udfs.BetradarUnifiedFeed.Plugin.InputDumping.Messages;
//using Udfs.BetradarUnifiedFeed.Plugin.Product.Messages;
//using Udfs.Common.Actor;
//using Udfs.Common.Messages;

//namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi
//{
//	public class ApiActor : BetradarUnifiedActorBase
//	{
//        private readonly IApiRequestsFactory _apiRequestFactory;
//        private readonly IApiResponsesParser _apiResponsesParser;
//        private readonly IApiCache _apiCache;
//        private readonly HttpClient _httpClient;
//        private readonly DumpsPlayerOptions _dumpsPlayerOptions;
//        private readonly Logger _logger;
//        private readonly ICanTell _adapterActor;
//        private readonly ICanTell _failureActor;
//        private readonly ICanTell _inputDumper;
//        private readonly ICanTell _feedActor;
//        private readonly ApiConfiguration _config;
//        private List<DateTimeOffset> _history;

//        public ApiActor(
//            IApiRequestsFactory apiRequestFactory,
//            IApiResponsesParser apiResponsesParser,
//            IApiCache apiCache,
//            HttpClient httpClient,
//            ActorMetadata<InputMessageDumperActor> inputDumperActorMetadata,
//            ActorMetadata<AdapterActor> adapterActorMeta,
//            ActorMetadata<FeedActor> feedActorMeta,
//            ActorMetadata<FailureSupervisorActor> failureActorMeta,
//            DumpsPlayerOptions dumpsPlayerOptions,
//            ApiConfiguration config,
//			LogFactory logFactory
//		) : base(logFactory)
//        {
//            _logger = logFactory.GetCurrentClassLogger();
//            _apiRequestFactory = apiRequestFactory;
//            _apiResponsesParser = apiResponsesParser;
//            _apiCache = apiCache;
//            _httpClient = httpClient;
//            _dumpsPlayerOptions = dumpsPlayerOptions;
//            _config = config;

//            _history = new List<DateTimeOffset>();

//            _inputDumper = Context.ActorSelection(inputDumperActorMetadata);
//            _adapterActor = Context.ActorSelection(adapterActorMeta);
//            _failureActor = Context.ActorSelection(failureActorMeta);
//            _feedActor = Context.ActorSelection(feedActorMeta);

//            Receive<IApiCommandCreateRequest>(msg => CreateApiRequest(msg));
//			ReceiveAsync<ApiCommand>(async msg => await PerformApiRequest(msg));
//			Receive<ParseApiResponse>(msg => ProcessApiResponse(msg));
//			Receive<ShutdownCommand>(msg => Shutdown());
//		}

//        private void CreateApiRequest(IApiCommandCreateRequest command)
//        {
//            try
//            {
//                var apiRequestMessage = _apiRequestFactory.CreateRequestMessage(command);
//                Self.Tell(apiRequestMessage, Self);

//                if (command is RequestOddsRecovery recoveryCommand)
//                {
//                    _feedActor.Tell(new OddsRecoveryRequested(
//                        productType: recoveryCommand.ProductType,
//                        requestId: recoveryCommand.RequestId
//                    ), Self);
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.Error(e, command.GetType().Name);
//            }
//        }

//        private async Task PerformApiRequest(ApiCommand message)
//        {
//            if (_dumpsPlayerOptions.IsEnabled) return;

//            try
//            {
//                if (_apiCache.Contains(message))
//                {
//                    _logger.Debug("{Action} RequestId={RequestId}", "SkipApiRequestByCacheReason",
//                        message.RequestId.ToString("N"));
//                    return;
//                }

//                if (message.IsRecovery)
//                {
//                    _history.Add(DateTimeOffset.UtcNow);

//                    if (TrySchedulerRecoveryMessage(message))
//                        return;
//                }

//                // TODO: Add retry logic
//                var request = new HttpRequestMessage(message.Method, message.Endpoint);

//                foreach (var header in message.CustomHeaders)
//                {
//                    request.Headers.Add(header.Key, header.Value);
//                }

//				// Requests are disabled
//				if (_config.DisableRemoteRequests) return;

//				// Calls api
//				var response = await _httpClient.SendAsync(request);

//                if (response.IsSuccessStatusCode)
//                {
//                    _apiCache.Add(message);

//                    Self.Tell(new ParseApiResponse(
//                        incomingId: Guid.NewGuid(),
//                        receivedOn: DateTimeOffset.UtcNow,
//                        data: await response.Content.ReadAsStringAsync(),
//                        language: message.Language,
//                        requestId: message.RequestId,
//						eventId: message.EventId,
//						lineService: message.LineService
//                    ), Self);

//                    _logger.Debug("{Action} {Method} {RequestUri} RequestId={RequestId}"
//                        , "ApiRequestSuccess", message.Method, message.Endpoint, message.RequestId);
//                }
//                else
//                {
//                    var responseContent = await response.Content.ReadAsStringAsync();

//                    _logger.Error(
//                        "{Action} {Method} {RequestUri} Status={Status} ResponseContent={ResponseContent}"
//                        , "ApiRequestError", message.Method, message.Endpoint, response.StatusCode, responseContent);

//                    RetryRecoveryMessage(message);
//                }
//            }

//            catch (Exception ex)
//            {
//                _logger.Error("{Action} {Method} {RequestUri} RequestId={RequestId} Error={Message}"
//                        , "ApiRequestError", message.Method, message.Endpoint, message.RequestId, ex.Message);

//                RetryRecoveryMessage(message);
//            }
//        }

//        private bool TrySchedulerRecoveryMessage(ApiCommand message)
//        {
//            _history = _history.Where(x => x > DateTimeOffset.UtcNow.AddMinutes(-_config.RetryDelayIntervalMin))
//                .OrderByDescending(x => x).ToList();
//            if (_history.Count > _config.RetryCountPerInterval)
//            {
//                var last = _history.FirstOrDefault();
//                var recoveryDelay = GetRecoveryDelay(last);

//                if (recoveryDelay > TimeSpan.Zero)
//                {
//                    Context.System.Scheduler.ScheduleTellOnce(
//                        recoveryDelay,
//                        Self,
//                        message,
//                        Self);

//                    _logger.Warn($"Recovery was scheduled at {DateTime.UtcNow.Add(recoveryDelay).ToString("HH:mm:ss")}");

//                    return true;
//                }
//            }

//            return false;
//        }

//        private void RetryRecoveryMessage(ApiCommand message)
//        {
//            if (message.IsRecovery)
//            {
//                Context.System.Scheduler.ScheduleTellOnce(
//                    TimeSpan.FromSeconds(2),
//                    Self,
//                    message,
//                    Self);
//            }
//        }

//        private TimeSpan GetRecoveryDelay(DateTimeOffset? last)
//        {
//            if (last.HasValue)
//            {
//                var difference = DateTimeOffset.UtcNow.Subtract(last.Value);
//                return TimeSpan.FromMinutes(_config.RetryDelayIntervalMin) - difference;
//            }

//            return TimeSpan.Zero;
//        }

//        private void ProcessApiResponse(ParseApiResponse response)
//        {
//            try
//            {
//                var parsedApiResponse = _apiResponsesParser.ParseResponse(response);

//                DumpApiResponse(response, parsedApiResponse);
//                PublishApiResponse(parsedApiResponse);

//                switch (parsedApiResponse)
//                {
//                    case ApiResponseParsingFailed _:
//                        _logger.Error("{Action} IncomingId={IncomingId} RequestId={RequestId}",
//                            "ApiResponseParsingFailed", response.IncomingId, response.RequestId);
//                        break;
//                    default:
//                        _logger.Debug("{Action} IncomingId={IncomingId} RequestId={RequestId}",
//                            "ApiResponseParsingSuccess", response.IncomingId, response.RequestId);
//                        break;
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.Error(e, "{Action} IncomingId={IncomingId} RequestId={RequestId}",
//                    "ApiResponseParsingError", response.IncomingId, response.RequestId);

//                _failureActor.Tell(new UnexpectedErrorOccured(
//                    failureTrigger: response,
//                    failureReason: e,
//					lineService: response.LineService
//				), Self);
//            }
//        }

//        private void PublishApiResponse(IApiResponseParsingResult parsedApiResponse)
//        {
//            switch (parsedApiResponse)
//            {
//                case ApiResponseParsed apiResponseParsed:
//                    if (apiResponseParsed.CanBeAdapted())
//                    {
//                        _adapterActor.Tell(apiResponseParsed, Self);
//                    }

//                    break;
//                case ApiResponseParsingFailed apiResponseParsingFailed:
//                    _failureActor.Tell(apiResponseParsingFailed, Self);
//                    break;
//            }
//        }

//        private void DumpApiResponse(ParseApiResponse response, IApiResponseParsingResult parsedApiResponse)
//        {
//            var dump = InputMessageDump.Create(parsedApiResponse.GetDumpMeta(), response.Data);
//            _inputDumper.Tell(dump, Self);
//        }

//        private void Shutdown()
//        {
//            Context.Stop(Self);
//        }
//    }
//}