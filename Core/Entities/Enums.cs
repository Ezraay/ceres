

namespace Ceres.Core.Enums
{
    public class EndServerBattleReasons{
        public const string
            Player1Left = "Player1Left",
            Player2Left = "Player2Left",
            Player1Win = "Player1Win",
            Player2Win = "Player1Win",
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