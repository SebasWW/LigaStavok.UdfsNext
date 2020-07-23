namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    public interface IFeedMessage
    {
        ProductType Product { get; }
        string GetEventId();
    }
}