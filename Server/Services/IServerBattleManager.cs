using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;

public interface IServerBattleManager
{
    ConcurrentDictionary<Guid,ServerBattle> ServerBattles();
    void EndServerBattle(Guid gameId, string reason);
    ServerBattle? FindServerBattleById(Guid gameId);
    ServerBattle GetServerBattle();
    string JoinBattle(Guid battleId, ServerPlayer serverPlayer);
}