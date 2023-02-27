
using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

public interface ISignalRService
{
    ConcurrentDictionary<string,GameUser> LobbyUsers();
    void SendServerBattleEnded(Guid gameId, string reason);
    void SendListOfGamesUpdated(string[] games);
    GameUser? FindGameUserByConnectionId(string connectionId);
    void UpdatePlayersName(string battleId, string? player1Name, string? player2Name);
    void OnPlayerLeftGame(string contextConnectionId);
    string JoinGame(Guid gameIdGuid, Guid userIdGuid);
    void OnPlayerSentCommand(Guid gameIdGuid, Guid userIdGuid, IClientCommand command);
    void ClientConnectedToLobby(string contextConnectionId);
    void ClientDisconnectedFromLobby(string contextConnectionId);
    void UserSentMessage(string contextConnectionId, string userName, string message);
    void ChangeUserName(string contextConnectionId, string newName);
    void UserIsReadyToPlay(string contextConnectionId, string userName, bool ready);
}