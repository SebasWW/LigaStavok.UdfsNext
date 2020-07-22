﻿namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public enum GameEventStatus
    {
        /// <summary>
        ///    Indicates that the associated sport event has not yet been started
        /// </summary>
        NotStarted = 0,

        /// <summary>
        ///    Indicates that the associated sport event is currently live (the match could be over-time, extended time or between periods too)
        /// </summary>
        Live = 1,

        /// <summary>
        ///    Indicates that the associated sport event has already been started, but is currently suspended
        /// </summary>
        Suspended = 2,

        /// <summary>
        ///    Indicates that the associated sport event has already ended according to our own data, the final results may not be ready yet
        /// </summary>
        Ended = 3,

        /// <summary>
        ///    Indicates that the associated sport event has already ended and no more changes for this event will be transmitted by the system (event closed)
        /// </summary>
        Closed = 4,

        /// <summary>
        ///    The sport event has been cancelled, the event will not take place, there will be no results
        /// </summary>
        Cancelled = 5,

        /// <summary>
        ///    Sportradar aborts scouting the match - this means there will be no live reporting; the match will likely take place anyhow,
        ///    and after the match has been played Sportradar will likely enter the results and the match will be moved to closed/finished
        /// </summary>
        Abandoned = 6,

        /// <summary>
        ///    	If a match has passed its scheduled start time but is delayed, unknown when it will start
        /// </summary>
        /// <remarks>
        ///    This is something that often happens in Tennis
        /// </remarks>
        Delayed = 7,

        /// <summary>
        ///    If a hitherto unsupported sport-event-status is received
        /// </summary>
        Unknown = 8,
    }
}