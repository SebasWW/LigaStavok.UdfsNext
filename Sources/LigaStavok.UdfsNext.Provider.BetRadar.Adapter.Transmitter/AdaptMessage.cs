using System;
using System.ComponentModel;
using Udfs.Common.Messages;
using Udfs.Common.Primitives;
using Udfs.Transmitter.Messages.Identifiers;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    [ImmutableObject(true)]
    public class AdaptMessage : IHaveHeader
    {
        #region Header

        public Guid IncomingId { get; }

        public DateTimeOffset ReceivedOn { get; }

        #endregion

        public Language Language { get; }

        public object Message { get; }

		public LineService LineService { get; }

        public AdaptMessage(Guid incomingId, DateTimeOffset receivedOn, Language language, object message, LineService lineService)
        {
            IncomingId = incomingId;
            ReceivedOn = receivedOn;
            Language = language;
            Message = message;
			LineService = lineService;
		}
    }
}