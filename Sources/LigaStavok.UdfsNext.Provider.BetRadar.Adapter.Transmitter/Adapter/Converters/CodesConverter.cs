using System.Collections.Generic;
using Udfs.Transmitter.Messages;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters
{
    public class CodesConverter
    {
        private static readonly Dictionary<string, VoidReasonCode> VoidReasonsMap =
            new Dictionary<string, VoidReasonCode>
            {
                {"0", VoidReasonCode.Other},
                {"1", VoidReasonCode.NoGoalscorer},
                {"2", VoidReasonCode.CorrectScoreMissing},
                {"3", VoidReasonCode.ResultUnverifiable},
                {"4", VoidReasonCode.FormatChange},
                {"5", VoidReasonCode.CancelledEvent},
                {"6", VoidReasonCode.MissingGoalscorer},
                {"7", VoidReasonCode.MatchEndedInWalkover},
                {"8", VoidReasonCode.DeadHeat},
                {"9", VoidReasonCode.RetiredOrDefaulted},
                {"10", VoidReasonCode.EventAbandoned},
                {"11", VoidReasonCode.EventPostponed},
                {"12", VoidReasonCode.IncorrectOdds},
                {"13", VoidReasonCode.IncorrectStatistics},
                {"14", VoidReasonCode.NoResultAssignable},
                {"15", VoidReasonCode.ClientSideSettlementNeeded},
                {"16", VoidReasonCode.StartingPitcherChanged }
            };

        public static VoidReasonCode? GetVoidReasonByReasonId(string reasonId)
        {
            if (string.IsNullOrWhiteSpace(reasonId)) return null;

            if (VoidReasonsMap.TryGetValue(reasonId.Trim(), out var code))
                return code;

            return null;
        }
    }
}