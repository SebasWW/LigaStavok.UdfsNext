using System;
using System.ComponentModel;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class TournamentLength
    {
        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        
        public static TournamentLength Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new TournamentLength
            {
                EndDate   = dynamicXml.EndDate<DateTimeOffset?>(),
                StartDate = dynamicXml.StartDate<DateTimeOffset?>()
            };

            return builder;
        }
    }
}