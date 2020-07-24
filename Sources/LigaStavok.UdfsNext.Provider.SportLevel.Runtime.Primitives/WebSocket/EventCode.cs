using System;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebSocket
{
	public static class EventCode
	{
		public const string HEARTBEAT = "HEARTBEAT";

		public const string MARKET = "MARKET";

		public const string APP_STOP = "APP_STOP";
		public const string BALL_IN_GAME = "BALL_IN_GAME";
		public const string BETSTOP = "BETSTOP";
		public const string BETSTART = "BETSTART";
		public const string FINISH_GAME = "FINISH_GAME";
		public const string FINISH_PERIOD = "FINISH_PERIOD";
		public const string GAME_INTERRUPTED = "GAME_INTERRUPTED";
		public const string GAME_START_DELAYED = "GAME_START_DELAYED";
		public const string MEDICAL_TIMEOUT = "MEDICAL_TIMEOUT";
		public const string PLAYERS_ON_PITCH = "PLAYERS_ON_PITCH";
		public const string PLAYER_WITHDRAWAL = "PLAYER_WITHDRAWAL";
		public const string POINT = "POINT";
		public const string POSESSION_2 = "POSESSION_2";
		public const string POSESSION_1 = "POSESSION_1";
		public const string SCOUT_OFFLINE = "SCOUT_OFFLINE";
		public const string STARTING_SIDE = "STARTING_SIDE";
		public const string START_PERIOD = "START_PERIOD";
		public const string SET = "SET";
		public const string TIMEOUT = "TIMEOUT";
		public const string WARM_UP = "WARM_UP";
		public const string CALCULATION_RESULT = "CALCULATION_RESULT";

	}
}
