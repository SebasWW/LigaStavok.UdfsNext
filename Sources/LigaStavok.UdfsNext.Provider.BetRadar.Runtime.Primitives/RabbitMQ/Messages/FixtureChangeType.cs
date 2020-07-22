namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public enum FixtureChangeType
    {
        /// <summary>
        ///    A new sport event has been added
        /// </summary>
        /// <remarks>
        ///    Typically used for events that are created and will start in the near-term
        /// </remarks>
        New = 1,

        /// <summary>
        ///    The start time of the sport event has changed
        /// </summary>
        StartTime = 2,

        /// <summary>
        ///    The sport event has been canceled
        /// </summary>
        Cancelled = 3,

        /// <summary>
        ///    Other various changes to the fixture
        /// </summary>
        Other = 4,

        /// <summary>
        ///    Coverage of the sport event has been changed
        /// </summary>
        Coverage = 5
    }
}