namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    /// <summary>
    ///    Marks queue message as stateful.
    /// </summary>
    public interface IStatefulMessage : IFeedMessage
    {
        int? RequestId { get; set; }
    }
}