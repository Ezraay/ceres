using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

public interface IBattleService
{
    // ServerBattle AllocateServerBattle();
    
    void StartBattle(GameUser user1, GameUser user2);
    void StopBattle(Guid battleId, string reason);
    void PlayerLeftGame(string connectionId);
    // void PlayerAction(ServerPlayer player, IServerAction action);
    // GameUser? FindGameUserByConnectionId(string connectionId);
    // ConcurrentDictionary<string,GameUser> GameUsers();
    void SendServerBattleEnded(Guid gameId, string reason);
    // void SendListOfGamesUpdated(string[] games);
    
}
