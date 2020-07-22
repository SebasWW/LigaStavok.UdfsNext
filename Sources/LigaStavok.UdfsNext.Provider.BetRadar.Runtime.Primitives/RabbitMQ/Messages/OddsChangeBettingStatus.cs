namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public enum OddsChangeBettingStatus
    {
        /// <summary>
        ///    The betting status is not known
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///    The betting status indicating the goal was scored
        /// </summary>
        Goal = 1,

        /// <summary>
        ///    	The betting status indicating a dangerous free kick
        /// </summary>
        DangerousFreeKick = 2,

        /// <summary>
        ///    The betting status indicating a dangerous goal position
        /// </summary>
        DangerousGoalPosition = 3,

        /// <summary>
        ///    The betting status indicating a possible boundary
        /// </summary>
        PossibleBoundary = 4,

        /// <summary>
        ///    The betting status indicating a possible checkout
        /// </summary>
        PossibleCheckout = 5,

        /// <summary>
        ///    The betting status indicating an in-game penalty
        /// </summary>
        IngamePenalty = 6
    }
}