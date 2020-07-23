using System;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Extensions
{
    public static class TimeExtensions
    {
        public static int ParseMinutesFrom(this string s, Rounding mode)
        {
            var fractions = s.Split(':');
            if (mode == Rounding.AwayFromZero)
            {
                var min = int.Parse(fractions[fractions.Length - 2]);
                var sec = int.Parse(fractions[fractions.Length - 1]);
                return sec > 0 ? min + 1 : min;
            }
            return int.Parse(fractions[fractions.Length - 2]);
        }

        public static DateTime GetLastBusinessDayOfMonth(this DateTimeOffset value)
        {
            var lastDayOfMonth = new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));

            if (lastDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
                lastDayOfMonth = lastDayOfMonth.AddDays(-2);
            else if (lastDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);

            return lastDayOfMonth;
        }
    }

    public enum Rounding
    {
        // When a number is halfway between two others, it is rounded toward the nearest less number.
        ToZero = 0,
        // When a number is halfway between two others, it is rounded toward the nearest number that is away from zero.
        AwayFromZero = 1
    }
}
