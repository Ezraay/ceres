using System;
using Ceres.Core.BattleSystem;

namespace Ceres.Core.Entities
{
    public class GameUser : ServerPlayer
    {
        public string ConnectionId { get; set; } = "";
        // public string UserName { get; set; } = "";
        public Guid UserId { get; set; }
        public bool ReadyToPlay { get; set; } = false;
        public Guid GameId { get; set; }
    }
    
}