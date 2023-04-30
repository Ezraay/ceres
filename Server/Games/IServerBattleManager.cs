using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Server.Games;

public interface IServerBattleManager
{
    ConcurrentDictionary<Guid,ServerBattle> ServerBattles();
    // void EndServerBattle(Guid gameId, string reason);
    ServerBattle? FindServerBattleById(Guid gameId);
    ServerBattle AllocateServerBattle(IPlayer player1, IPlayer player2);
    void DeallocateServerBattle(ServerBattle battle);
}