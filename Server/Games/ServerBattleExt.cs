using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;


namespace Ceres.Core.BattleSystem{

    public static class ServerBattleExtensions{

        public static GameUser? GetGameUserById(this ServerBattle battle, Guid id){
            if ((battle.Player1 as GameUser)?.UserId == id)
                return (GameUser)battle.Player1;
            if ((battle.Player2 as GameUser)?.UserId == id)
                return (GameUser)battle.Player2;
            return null;
        }

        public static ServerPlayer? GetServerPlayerById(this ServerBattle battle, Guid id){
            return GetGameUserById(battle, id);
            
        }

    }
}