using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LigaStavok.UdfsNext.Dumps;
using LigaStavok.UdfsNext.Provider.BetRadar.Adapter.DataFlow;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using LigaStavok.UdfsNext.Provider.BetRadar.WebApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Udfs.Common.Actor;
using Udfs.Transmitter.Messages;
using Udfs.Transmitter.Messages.Interfaces;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter
{

	public sealed class TransmitterAdapter : IProviderAdapter
    {
		private readonly AdapterDataFlow adapterDataFlow;

		public TransmitterAdapter(
            ILogger<TransmitterAdapter> logger,
            AdapterDataFlow adapterDataFlow

        ) 
        {
			this.adapterDataFlow = adapterDataFlow;
		}

        public Task SendWebApiMessageAsync(MessageContext<ApiResponseParsed> messageContext)
        {
            adapterDataFlow.Post(messageContext);
            return Task.CompletedTask;
        }


        public Task SendRabbitMessageAsync(MessageContext<FeedMessageReady> messageContext)
        {
            adapterDataFlow.Post(messageContext);
            return Task.CompletedTask;
        }


        //private async Task AdaptMessage(AdaptMessage message)
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {

        //        _failureSupervisor.Tell(new UnexpectedErrorOccured(
        //            failureTrigger: message,
        //            failureReason: ex,
        //            lineService: message.LineService
        //        ), Self);
        //    }
        //}      
    }
}