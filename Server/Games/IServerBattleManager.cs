using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;

namespace Ceres.Server.Games;

public interface IServerBattleManager
{
    ConcurrentDictionary<Guid,ServerBattle> ServerBattles();
    void EndServerBattle(Guid gameId, string reason);
    ServerBattle? FindServerBattleById(Guid gameId);
    ServerBattle AllocateServerBattle();
}