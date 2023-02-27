
using System.Collections.Concurrent;
using Ceres.Core.Entities;

public interface ISignalRService
{
    ConcurrentDictionary<string,GameUser> LobbyUsers();
    void SendServerBattleEnded(Guid gameId, string reason);
    void SendListOfGamesUpdated(string[] games);
    GameUser? FindGameUserByConnectionId(string connectionId);
    void UpdatePlayersName(string battleId, string? player1Name, string? player2Name);
}