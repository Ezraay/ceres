

namespace Ceres.Core.Entities
{
    public class EndServerBattleReasons{
        public const string
            YouLost = "YouLost",
            YouWon = "YouWon",
            ReasonUnknown = "ReasonUnknown";
    }

    public class JoinGameResults{
        public const string
            NoGameFound = "NoGameFound",
            JoinedAsPlayer1 = "JoinedAsPlayer1",
            JoinedAsPlayer2 = "JoinedAsPlayer2",
            JoinedAsSpectator = "JoinedAsSpectator";

    }
}