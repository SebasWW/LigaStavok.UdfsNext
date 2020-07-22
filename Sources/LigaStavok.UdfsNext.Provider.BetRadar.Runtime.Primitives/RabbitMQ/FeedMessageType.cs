namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    public static class FeedMessageType
    {
        public const string Alive                 = "alive";
        public const string BetCancel             = "bet_cancel";
        public const string BetSettlement         = "bet_settlement";
        public const string BetStop               = "bet_stop";
        public const string FixtureChange         = "fixture_change";
        public const string OddsChange            = "odds_change";
        public const string ProductDown           = "product_down";
        public const string RollbackBetSettlement = "rollback_bet_settlement";
        public const string RollbackBetCancel     = "rollback_bet_cancel";
        public const string SnapshotComplete      = "snapshot_complete";

        public const string Unknown               = "unknown";
    }
}