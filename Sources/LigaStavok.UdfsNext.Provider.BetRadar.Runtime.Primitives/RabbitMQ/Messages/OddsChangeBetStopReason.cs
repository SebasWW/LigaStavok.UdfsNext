namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages
{
    public enum OddsChangeBetStopReason
    {
        /// <summary>
        ///    The reason for the bet-stop is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///    Betting has been stopped due to possible goal
        /// </summary>
        PossibleGoal = 1,

        /// <summary>
        ///    Betting has been stopped due to possible red card
        /// </summary>
        PossibleRedCard = 2,

        /// <summary>
        ///    Betting has been stopped because connection to the scout system has been lost
        /// </summary>
        ScoutLost = 3,

        /// <summary>
        ///    Betting has been stopped due to possible goal by home team
        /// </summary>
        PossibleGoalHome = 4,

        /// <summary>
        ///    Betting has been stopped due to possible goal by away team
        /// </summary>
        PossibleGoalAway = 5,

        /// <summary>
        ///    Betting has been stopped due to possible red card shown to home team
        /// </summary>
        PossibleRedCardHome = 6,

        /// <summary>
        ///    Betting has been stopped due to possible read card shown to away team
        /// </summary>
        PossibleRedCardAway = 7,

        /// <summary>
        ///    Betting has been stopped due to possible penalty
        /// </summary>
        PossiblePenalty = 8,

        /// <summary>
        ///    Betting has been stopped due to possible penalty to home team
        /// </summary>
        PossiblePenaltyHome = 9,

        /// <summary>
        ///    Betting has been stopped due to possible penalty to away team
        /// </summary>
        PossiblePenaltyAway = 10,

        /// <summary>
        ///    TBD
        /// </summary>
        ConnectedToSupervisor = 11,

        /// <summary>
        ///    Betting has been stopped because the match has ended
        /// </summary>
        MatchEnded = 12,

        /// <summary>
        ///    Betting has been stopped because game-point was reached
        /// </summary>
        Gamepoint = 13,

        /// <summary>
        ///    Betting has been stopped due to tie-break
        /// </summary>
        Tiebreak = 14,

        /// <summary>
        ///    Betting has been stopped due to possible direct foul by the home team
        /// </summary>
        PossibleDirectFoulHome = 15,

        /// <summary>
        ///    Betting has been stopped due to possible direct foul by the away team
        /// </summary>
        PossibleDirectFoulAway = 16,

        /// <summary>
        ///    Betting has been stopped due to possible direct foul
        /// </summary>
        PossibleDirectFoul = 17,

        /// <summary>
        ///    Betting has been stopped due to dangerous free kick by the home team
        /// </summary>
        DangerousFreeKickHome = 18,

        /// <summary>
        ///    Betting has been stopped due to dangerous free kick by the away team
        /// </summary>
        DangerousFreeKickAway = 19,

        /// <summary>
        ///    Betting has been stopped due to dangerous ball position (home team)
        /// </summary>
        DangerousGoalPositionHome = 20,

        /// <summary>
        ///    Betting has been stopped due to dangerous ball position (away team)
        /// </summary>
        DangerousGoalPositionAway = 21,

        /// <summary>
        ///    Betting has been stopped due to goal review
        /// </summary>
        GoalUnderReview = 22,

        /// <summary>
        ///    Betting has been stopped due to score review
        /// </summary>
        ScoreUnderReview = 23,

        /// <summary>
        ///    TBD
        /// </summary>
        Disconnection = 24,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleCheckout = 25,

        /// <summary>
        ///    TBD
        /// </summary>
        MultipleSuspensions = 26,

        /// <summary>
        ///    Betting has been stopped due to possible dangerous free kick
        /// </summary>
        PossibleDangerousFreeKick = 27,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleDangerousGoalPosition = 28,

        /// <summary>
        ///    Betting has been stopped due to possible touch-down by home team
        /// </summary>
        PossibleTouchdownHome = 29,

        /// <summary>
        ///    Betting has been stopped due to possible touch-down by the away team
        /// </summary>
        PossibleTouchdownAway = 30,

        /// <summary>
        ///    Betting has been stopped due to possible goal from a place kick(home team)
        /// </summary>
        PossibleFieldgoalHome = 31,

        /// <summary>
        ///    Betting has been stopped due to possible goal from a place kick(away team)
        /// </summary>
        PossibleFieldgoalAway = 32,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleSafetyHome = 33,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleSafetyAway = 34,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleTurnoverHome = 35,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleTurnoverAway = 36,

        /// <summary>
        ///    Betting has been stopped due to review of match video
        /// </summary>
        VideoReview = 37,

        /// <summary>
        ///    TBD
        /// </summary>
        RedzoneHome = 38,

        /// <summary>
        ///    TBD
        /// </summary>
        RedzoneAway = 39,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleBoundary = 40,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleWicket = 41,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleChallengeHome = 42,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleChallengeAway = 43,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleTurnover = 44,

        /// <summary>
        ///    TBD
        /// </summary>
        UnknownOperator = 45,

        /// <summary>
        ///    TBD
        /// </summary>
        Freeball = 46,

        /// <summary>
        ///    TBD
        /// </summary>
        DeepBall = 47,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleRun = 48,

        /// <summary>
        ///    TBD
        /// </summary>
        Maintenance = 49,

        /// <summary>
        ///    TBD
        /// </summary>
        BaseHitDeleted = 50,

        /// <summary>
        ///    TBD
        /// </summary>
        MatchDelayed = 51,

        /// <summary>
        ///    TBD
        /// </summary>
        MatchPostponed = 52,

        /// <summary>
        ///    TBD
        /// </summary>
        ScoutDisconnectionTvSignal = 53,

        /// <summary>
        ///    TBD
        /// </summary>
        PossiblePenaltyOffsetting = 54,

        /// <summary>
        ///    TBD
        /// </summary>
        PossiblePuntHome = 55,

        /// <summary>
        ///    TBD
        /// </summary>
        PossiblePuntAway = 56,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleFourthDownAttemptHome = 57,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleFourthDownAttemptAway = 58,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleOnsideKickHome = 59,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleOnsideKickAway = 60,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleChallenge = 61,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleCard = 62,

        /// <summary>
        ///    TBD
        /// </summary>
        DelayedPenalty = 63,

        /// <summary>
        ///    TBD
        /// </summary>
        ShootoutBegins = 64,

        /// <summary>
        ///    TBD
        /// </summary>
        EmptyNet = 65,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleTryHome = 66,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleTryAway = 67,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleDropGoalHome = 68,

        /// <summary>
        ///    TBD
        /// </summary>
        PossibleDropGoalAway = 69,

        /// <summary>
        ///    Betting was stopped due to possible card shown to home team
        /// </summary>
        PossibleCardHome = 70,

        /// <summary>
        ///    Betting was stopped due to possible card shown to away team
        /// </summary>
        PossibleCardAway = 71,

        /// <summary>
        ///    Betting was stopped due to possible penalty to home team (hockey)
        /// </summary>
        PossiblePenaltyHomeHockey = 72,

        /// <summary>
        ///    Betting was stopped due to possible penalty to away team (hockey)
        /// </summary>
        PossiblePenaltyAwayHockey = 73,

        /// <summary>
        ///    Betting was stopped due to delayed penalty to home team (hockey)
        /// </summary>
        DelayedPenaltyHomeHockey = 74,

        /// <summary>
        ///    Betting was stopped due to delayed penalty to away team (hockey)
        /// </summary>
        DelayedPenaltyAwayHockey = 75,

        /// <summary>
        ///    TBD
        /// </summary>
        TwoManAdvantageHome = 76,

        /// <summary>
        ///    TBD
        /// </summary>
        TwoManAdvantageAway = 77
    }
}