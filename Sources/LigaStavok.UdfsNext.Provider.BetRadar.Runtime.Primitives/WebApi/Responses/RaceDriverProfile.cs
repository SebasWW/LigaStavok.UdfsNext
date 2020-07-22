using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class RaceDriverProfile
    {
        public Car Car { get; set; }

        public RaceDriver RaceDriver { get; set; }

        public Team RaceTeam { get; set; }

        public static RaceDriverProfile Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new RaceDriverProfile
            {
                Car        = Responses.Car.Parse(dynamicXml.Car),
                RaceDriver = Responses.RaceDriver.Parse(dynamicXml.RaceDriver),
                RaceTeam   = Team.Parse(dynamicXml.RaceTeam)
            };

            return builder;
        }
    }
}