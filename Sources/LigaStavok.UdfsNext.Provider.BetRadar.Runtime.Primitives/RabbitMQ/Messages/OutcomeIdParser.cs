using System.Text.RegularExpressions;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public static class OutcomeIdParser
    {
        private static readonly Regex PlayerScoreOutcomeRegex=new Regex(@"pre:playerprops:\d+:\d+:(?<id>\d)",RegexOptions.Compiled);

        public static string ParseId(string srcId)
        {
            var playerScoreMatch = PlayerScoreOutcomeRegex.Match(srcId);
            if (!playerScoreMatch.Success)
                return srcId;
            // У исходов с playerprops id должен быть извлечен из цифры с количеством очков
            return playerScoreMatch.Groups["id"].Value;
        }
    }
}