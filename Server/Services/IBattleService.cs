using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;

public interface IBattleService
{
    ServerBattle AllocateServerBattle();
    // string JoinBattle(Guid battleId, GameUser gameUser);
    void StartBattle(ServerBattle battle);
    void StopBattle(Guid battleId, string reason);
    void PlayerLeftGame(string connectionId);
    // void PlayerAction(ServerPlayer player, IServerAction action);
}
