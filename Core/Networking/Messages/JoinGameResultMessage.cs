using Ceres.Core.BattleSystem;
using Ceres.Core.Enums;

namespace Ceres.Core.Networking.Messages
{
    public class JoinGameResultMessage : INetworkMessage
    {
        public ClientBattleStartConfig Config;
        
        public JoinGameResultMessage(ClientBattleStartConfig config)
        {
            Config = config;
        }

        public string MessageName => "JoinGame";
    }
}