using System;
using System.Collections.Generic;
using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Converters
{
	public static class MatchStatusConverter
	{
		public static SportLevelGameEventStatus Convert(string matchStatus)
		{
			Statuses.TryGetValue(matchStatus, out var res);
			return res;
		}

		private static Dictionary<string, SportLevelGameEventStatus> Statuses => statusesDictionary.Value;

		private static readonly Lazy<Dictionary<string, SportLevelGameEventStatus>> statusesDictionary 
			= new Lazy<Dictionary<string, SportLevelGameEventStatus>>(() => ConstructStatuses(), true);

		private static Dictionary<string, SportLevelGameEventStatus> ConstructStatuses()
		{
			var ld = new Dictionary<string, SportLevelGameEventStatus>();

			ld.Add(EventCode.GAME_START_DELAYED, new SportLevelGameEventStatus("delayed"));
			ld.Add(EventCode.PLAYERS_ON_PITCH, new SportLevelGameEventStatus("OnCourt"));
			ld.Add(EventCode.WARM_UP, new SportLevelGameEventStatus("WarmingUp"));
			ld.Add(EventCode.TIMEOUT, new SportLevelGameEventStatus("PlaySuspended"));
			ld.Add(EventCode.MEDICAL_TIMEOUT, new SportLevelGameEventStatus("MedicalTimeout"));
			ld.Add(EventCode.PLAYER_WITHDRAWAL, new SportLevelGameEventStatus("retired"));
			ld.Add(EventCode.GAME_INTERRUPTED, new SportLevelGameEventStatus("PlaySuspended"));
			ld.Add(EventCode.FINISH_GAME, new SportLevelGameEventStatus("ended"));

			return ld;
		}
	}
}
