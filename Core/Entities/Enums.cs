

namespace Ceres.Core.Enums
{
    public enum EndServerBattleReasons{
        Player1Left,
        Player2Left
    }

    public class JoinGameResults{
        public const string
            NoGameFound = "NoGameFound",
            JoinedAsPlayer1 = "JoinedAsPlayer1",
            JoinedAsPlayer2 = "JoinedAsPlayer2",
            JoinedAsSpectator = "JoinedAsSpectator";

    }
}