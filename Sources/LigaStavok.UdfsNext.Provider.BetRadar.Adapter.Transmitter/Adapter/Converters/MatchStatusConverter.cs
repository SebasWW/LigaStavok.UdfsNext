using System;
using System.Collections.Generic;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters
{
	public static class MatchStatusConverter
	{
		public static BetRadarGameEventStatus Convert(int matchStatus)
		{
			return Statuses[matchStatus];
		}

		public static BetRadarGameEventStatus Convert(string matchStatus)
		{
			return Codes[matchStatus];
		}

		private static Dictionary<string, BetRadarGameEventStatus> Codes => codesDictionary.Value;

		private static readonly Lazy<Dictionary<string, BetRadarGameEventStatus>> codesDictionary 
			= new Lazy<Dictionary<string, BetRadarGameEventStatus>>(
				() => 
				Statuses
					.GroupBy(t => t.Value.Value, (t, arr) => arr.First().Value)
					.ToDictionary(obj => obj.Value, obj => obj),
			true
		);

		private static Dictionary<int, BetRadarGameEventStatus> Statuses => statusesDictionary.Value;

		private static readonly Lazy<Dictionary<int, BetRadarGameEventStatus>> statusesDictionary = new Lazy<Dictionary<int, BetRadarGameEventStatus>>(() => ConstructStatuses(), true);

		private static Dictionary<int,BetRadarGameEventStatus> ConstructStatuses()
		{
			var ld = new Dictionary<int, BetRadarGameEventStatus>();

			// Todo: it need to optimise dublicated class instances
			ld.Add(0, new BetRadarGameEventStatus("not_started")); //	Матч не начался
			ld.Add(1, new BetRadarGameEventStatus("1p")); //	1-й период
			ld.Add(2, new BetRadarGameEventStatus("2p")); //	2-й период
			ld.Add(3, new BetRadarGameEventStatus("3p")); //	3-й период
			ld.Add(4, new BetRadarGameEventStatus("4p")); //	4-й период
			ld.Add(5, new BetRadarGameEventStatus("5p")); //	5-й период
			ld.Add(6, new BetRadarGameEventStatus("1h")); //	1-я половина
			ld.Add(7, new BetRadarGameEventStatus("2h")); //	2-я половина
			ld.Add(8, new BetRadarGameEventStatus("1set")); //	1-ый сет
			ld.Add(9, new BetRadarGameEventStatus("2set")); //	2-ой сет
			ld.Add(10, new BetRadarGameEventStatus("3set")); //	3-ий сет
			ld.Add(11, new BetRadarGameEventStatus("4set")); //	4-ый сет
			ld.Add(12, new BetRadarGameEventStatus("5set")); //	5-ый сет
			ld.Add(13, new BetRadarGameEventStatus("1q")); //	1-я четверть
			ld.Add(14, new BetRadarGameEventStatus("2q")); //	2-я четверть
			ld.Add(15, new BetRadarGameEventStatus("3q")); //	3-я четверть
			ld.Add(16, new BetRadarGameEventStatus("4q")); //	4-я четверть
			ld.Add(17, new BetRadarGameEventStatus("gset")); //	Золотой сет
			ld.Add(20, new BetRadarGameEventStatus("started")); //	Начался
			ld.Add(21, new BetRadarGameEventStatus("inplay")); //	Розыгрыш очка
			ld.Add(22, new BetRadarGameEventStatus("starting")); //	Начинается
			ld.Add(30, new BetRadarGameEventStatus("paused")); //	перерыв
			ld.Add(31, new BetRadarGameEventStatus("paused")); //	
			ld.Add(32, new BetRadarGameEventStatus("awaiting_ot")); //	Ждем OT
			ld.Add(33, new BetRadarGameEventStatus("paused")); //	перерыв
			ld.Add(34, new BetRadarGameEventStatus("awaiting_pen")); //	ждем пенальти
			ld.Add(35, new BetRadarGameEventStatus("awaiting_pen")); //	ждем пенальти
			ld.Add(36, new BetRadarGameEventStatus("awaiting_pen")); //	ждем пенальти
			ld.Add(37, new BetRadarGameEventStatus("awaiting_gset")); //	ожидаем Золотой сет
			ld.Add(40, new BetRadarGameEventStatus("ot")); //	ОТ
			ld.Add(41, new BetRadarGameEventStatus("1p_ot")); //	1-й период ОТ
			ld.Add(42, new BetRadarGameEventStatus("2p_ot")); //	2-й период ОТ
			ld.Add(50, new BetRadarGameEventStatus("pen")); //	пенальти
			ld.Add(51, new BetRadarGameEventStatus("pen")); //	пенальти
			ld.Add(52, new BetRadarGameEventStatus("pen")); //	пенальти
			ld.Add(60, new BetRadarGameEventStatus("postponed")); //	перенесен
			ld.Add(61, new BetRadarGameEventStatus("delayed")); //	Отложен
			ld.Add(70, new BetRadarGameEventStatus("MatchAbandoned")); //	Матч отменен
			ld.Add(71, new BetRadarGameEventStatus("1g")); //	1-й гейм
			ld.Add(72, new BetRadarGameEventStatus("2g")); //	2-й гейм
			ld.Add(73, new BetRadarGameEventStatus("3g")); //	3-й гейм
			ld.Add(74, new BetRadarGameEventStatus("4g")); //	4-й гейм
			ld.Add(75, new BetRadarGameEventStatus("5g")); //	5-й гейм
			ld.Add(76, new BetRadarGameEventStatus("6g")); //	6-й гейм
			ld.Add(77, new BetRadarGameEventStatus("7g")); //	7-й гейм
			ld.Add(80, new BetRadarGameEventStatus("Interrupted")); //	Прерван
			ld.Add(81, new BetRadarGameEventStatus("PlaySuspended")); //	Матч приостановлен
			ld.Add(90, new BetRadarGameEventStatus("MatchAbandoned")); //	Отменен
			ld.Add(91, new BetRadarGameEventStatus("walkover")); //	Отказ участника до матча
			ld.Add(93, new BetRadarGameEventStatus("walkover")); //	Отказ участника до матча
			ld.Add(94, new BetRadarGameEventStatus("walkover")); //	Отказ участника до матча
			ld.Add(92, new BetRadarGameEventStatus("retired")); //	Отказ/дисквалификация участника во время матча
			ld.Add(95, new BetRadarGameEventStatus("retired")); //	Отказ/дисквалификация участника во время матча
			ld.Add(96, new BetRadarGameEventStatus("retired")); //	Отказ/дисквалификация участника во время матча
			ld.Add(97, new BetRadarGameEventStatus("retired")); //	Отказ/дисквалификация участника во время матча
			ld.Add(98, new BetRadarGameEventStatus("retired")); //	Отказ/дисквалификация участника во время матча
			ld.Add(99, new BetRadarGameEventStatus("unknown")); //	-
			ld.Add(100, new BetRadarGameEventStatus("ended")); //	Завершен
			ld.Add(110, new BetRadarGameEventStatus("after_ot")); //	ОТ завершен
			ld.Add(120, new BetRadarGameEventStatus("ended")); //	Завершен
			ld.Add(130, new BetRadarGameEventStatus("after_gset")); //	Золотой сет завершен
			ld.Add(141, new BetRadarGameEventStatus("1map")); //	1-я карта
			ld.Add(142, new BetRadarGameEventStatus("2map")); //	2-я карта
			ld.Add(143, new BetRadarGameEventStatus("3map")); //	3-я карта
			ld.Add(144, new BetRadarGameEventStatus("4map")); //	4-я карта
			ld.Add(145, new BetRadarGameEventStatus("5map")); //	5-я карта
			ld.Add(146, new BetRadarGameEventStatus("6map")); //	6-я карта
			ld.Add(147, new BetRadarGameEventStatus("7map")); //	7-я карта
			ld.Add(151, new BetRadarGameEventStatus("1g")); //	1-й гейм
			ld.Add(152, new BetRadarGameEventStatus("2g")); //	2-й гейм
			ld.Add(153, new BetRadarGameEventStatus("3g")); //	3-й гейм
			ld.Add(154, new BetRadarGameEventStatus("4g")); //	4-й гейм
			ld.Add(155, new BetRadarGameEventStatus("5g")); //	5-й гейм
			ld.Add(161, new BetRadarGameEventStatus("1end")); //	1-й энд
			ld.Add(162, new BetRadarGameEventStatus("2end")); //	2-й энд
			ld.Add(163, new BetRadarGameEventStatus("3end")); //	3-й энд
			ld.Add(164, new BetRadarGameEventStatus("4end")); //	4-й энд
			ld.Add(165, new BetRadarGameEventStatus("5end")); //	5-й энд
			ld.Add(166, new BetRadarGameEventStatus("6end")); //	6-й энд
			ld.Add(167, new BetRadarGameEventStatus("7end")); //	7-й энд
			ld.Add(168, new BetRadarGameEventStatus("8end")); //	8-й энд
			ld.Add(169, new BetRadarGameEventStatus("9end")); //	9-й энд
			ld.Add(170, new BetRadarGameEventStatus("10end")); //	10-й энд
			ld.Add(171, new BetRadarGameEventStatus("extra_end")); //	доп. энд
			ld.Add(301, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(302, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(303, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(304, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(305, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(306, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(401, new BetRadarGameEventStatus("1IT")); //	1-й Иннинг, Гости в нападении
			ld.Add(402, new BetRadarGameEventStatus("1IB")); //	1-й Иннинг, Хозяева в нападении
			ld.Add(403, new BetRadarGameEventStatus("2IT")); //	2-й Иннинг, Гости в нападении
			ld.Add(404, new BetRadarGameEventStatus("2IB")); //	2-й Иннинг, Хозяева в нападении
			ld.Add(405, new BetRadarGameEventStatus("3IT")); //	3-й Иннинг, Гости в нападении
			ld.Add(406, new BetRadarGameEventStatus("3IB")); //	3-й Иннинг, Хозяева в нападении
			ld.Add(407, new BetRadarGameEventStatus("4IT")); //	4-й Иннинг, Гости в нападении
			ld.Add(408, new BetRadarGameEventStatus("4IB")); //	4-й Иннинг, Хозяева в нападении
			ld.Add(409, new BetRadarGameEventStatus("5IT")); //	5-й Иннинг, Гости в нападении
			ld.Add(410, new BetRadarGameEventStatus("5IB")); //	5-й Иннинг, Хозяева в нападении
			ld.Add(411, new BetRadarGameEventStatus("6IT")); //	6-й Иннинг, Гости в нападении
			ld.Add(412, new BetRadarGameEventStatus("6IB")); //	6-й Иннинг, Хозяева в нападении
			ld.Add(413, new BetRadarGameEventStatus("7IT")); //	7-й Иннинг, Гости в нападении
			ld.Add(414, new BetRadarGameEventStatus("7IB")); //	7-й Иннинг, Хозяева в нападении
			ld.Add(415, new BetRadarGameEventStatus("8IT")); //	8-й Иннинг, Гости в нападении
			ld.Add(416, new BetRadarGameEventStatus("8IB")); //	8-й Иннинг, Хозяева в нападении
			ld.Add(417, new BetRadarGameEventStatus("9IT")); //	9-й Иннинг, Гости в нападении
			ld.Add(418, new BetRadarGameEventStatus("9IB")); //	9-й Иннинг, Хозяева в нападении
			ld.Add(419, new BetRadarGameEventStatus("EIT")); //	Доп.Иннинг, Гости в нападении
			ld.Add(420, new BetRadarGameEventStatus("EIB")); //	Доп.Иннинг, Хозяева в нападении
			ld.Add(421, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(422, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(423, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(424, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(425, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(426, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(427, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(428, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(429, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(430, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(431, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(432, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(433, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(434, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(435, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(436, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(437, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(438, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(439, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(443, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(445, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(505, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(509, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(510, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(511, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(512, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(524, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(525, new BetRadarGameEventStatus("paused")); //	Перерыв
			ld.Add(440, new BetRadarGameEventStatus("gball")); //	игра до победного очка
			ld.Add(441, new BetRadarGameEventStatus("6set")); //	6-й сет
			ld.Add(442, new BetRadarGameEventStatus("7set")); //	7-й сет
			ld.Add(514, new BetRadarGameEventStatus("8set")); //	8-й сет
			ld.Add(515, new BetRadarGameEventStatus("9set")); //	9-й сет
			ld.Add(516, new BetRadarGameEventStatus("10set")); //	10-й сет
			ld.Add(517, new BetRadarGameEventStatus("11set")); //	11-й сет
			ld.Add(518, new BetRadarGameEventStatus("12set")); //	12-й сет
			ld.Add(519, new BetRadarGameEventStatus("13set")); //	13-й сет
			ld.Add(444, new BetRadarGameEventStatus("ended")); //	Завершен
			ld.Add(508, new BetRadarGameEventStatus("ended")); //	Завершен
			ld.Add(513, new BetRadarGameEventStatus("ended")); //	Завершен
			ld.Add(501, new BetRadarGameEventStatus("1inn")); //	1-й иннинг
			ld.Add(502, new BetRadarGameEventStatus("1inn")); //	1-й иннинг
			ld.Add(531, new BetRadarGameEventStatus("1inn")); //	1-й иннинг
			ld.Add(503, new BetRadarGameEventStatus("2inn")); //	2-й иннинг
			ld.Add(504, new BetRadarGameEventStatus("2inn")); //	2-й иннинг
			ld.Add(532, new BetRadarGameEventStatus("2inn")); //	2-й иннинг
			ld.Add(520, new BetRadarGameEventStatus("3inn")); //	3-й иннинг
			ld.Add(521, new BetRadarGameEventStatus("3inn")); //	3-й иннинг
			ld.Add(533, new BetRadarGameEventStatus("3inn")); //	3-й иннинг
			ld.Add(522, new BetRadarGameEventStatus("4inn")); //	4-й иннинг
			ld.Add(523, new BetRadarGameEventStatus("4inn")); //	4-й иннинг
			ld.Add(534, new BetRadarGameEventStatus("4inn")); //	4-й иннинг
			ld.Add(535, new BetRadarGameEventStatus("5inn")); //	5-й иннинг
			ld.Add(536, new BetRadarGameEventStatus("6inn")); //	6-й иннинг
			ld.Add(537, new BetRadarGameEventStatus("7inn")); //	7-й иннинг
			ld.Add(538, new BetRadarGameEventStatus("8inn")); //	8-й иннинг
			ld.Add(539, new BetRadarGameEventStatus("9inn")); //	9-й иннинг
			ld.Add(506, new BetRadarGameEventStatus("sover")); //	Супер овер
			ld.Add(507, new BetRadarGameEventStatus("sover")); //	Супер овер
			ld.Add(526, new BetRadarGameEventStatus("sover")); //	Супер овер

			return ld;
		}
	}
}
