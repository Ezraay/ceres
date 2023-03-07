using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

public interface IServerBattleManager
{
    // string JoinBattle(Guid battleId, Guid userId, string connectionId);
    ConcurrentDictionary<Guid,ServerBattle> ServerBattles();
    void EndServerBattle(Guid gameId, string reason);
    ServerBattle? FindServerBattleById(Guid gameId);
    ServerBattle AllocateServerBattle();
    void StopBattle(Guid battleId, string reason);
    // void PlayerLeftGame(string connectionId);
    // ServerPlayer? FindServerPlayer(ServerBattle battle, Guid userId);
    // ServerPlayer? FindServerPlayer(Guid battleId, Guid userId);
    // ServerPlayer? FindServerPlayer(GameUser gameUser);
}