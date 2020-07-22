using System;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class PlayerProfile
    {
        public DateTimeOffset? GeneratedOn { get; set; }
       
        public Player Player { get; set; }

        public static PlayerProfile Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new PlayerProfile
            {
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset?>(),
                Player      = Responses.Player.Parse(dynamicXml.Player)
            };

            return builder;
        }
    }
}