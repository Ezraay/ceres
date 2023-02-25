using Ceres.Core.BattleSystem;
using Ceres.Core.Enums;

namespace Ceres.Core.Networking.Messages
{
    public class JoinGameResultMessage
    {
        public JoinGameResults JoinGameResults;
        public ClientBattleStartConfig Config;
        
        public JoinGameResultMessage(ClientBattleStartConfig config, JoinGameResults joinGameResults)
        {
            Config = config;
            JoinGameResults = joinGameResults;
        }
    }
}