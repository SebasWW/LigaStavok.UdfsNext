using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data
{
	public class EventData
	{
		[JsonProperty("TranslationID")]
		public string TranslationId { get; set; }

		[JsonProperty("EventCode")]
		public string EventCode { get; set; }

		[JsonProperty("GamePeriod")]
		public int? GamePeriod { get; set; }

		[JsonProperty("Playtime")]
		public long Playtime { get; set; }

		[JsonProperty("Timestamp")]
		public DateTimeOffset Timestamp { get; set; }

		[JsonProperty("ScoreHome")]
		public long? ScoreHome { get; set; }

		[JsonProperty("ScoreAway")]
		public long? ScoreAway { get; set; }

		[JsonProperty("EventNumber")]
		public long EventNumber { get; set; }

		[JsonProperty("HeartbeatNumber")]
		public long? HeartbeatNumber { get; set; }

		[JsonProperty("ScoutID")]
		public long ScoutId { get; set; }

		[JsonProperty("ProxyTimestamps")]
		public IEnumerable<DateTimeOffset> ProxyTimestamps { get; set; }

		[JsonProperty("extra")]
		public DataExtra Extra { get; set; }


		[JsonProperty("InputEventNumber")]
		public long InputEventNumber { get; set; }

		[JsonProperty("OutputEventNumber")]
		public long OutputEventNumber { get; set; }
	}
}
