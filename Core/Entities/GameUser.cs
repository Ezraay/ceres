using System;
using Ceres.Core.BattleSystem;

namespace Ceres.Core.Entities
{
    public class GameUser
    {
        public string LobbyConnectionId { get; set; } = "";
        public string GameConnectionId { get; set; } = "";
        public string UserName { get; set; } = "";
        public Guid UserId { get; set; } = Guid.Empty;
        public bool ReadyToPlay { get; set; } = false;
        public Guid GameId { get; set; } = Guid.Empty;
        public IPlayer ServerPlayer { get; set; }
        
    }
    
}