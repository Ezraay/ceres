using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

namespace Ceres.Server.Services;

public interface ISignalRService
{
    // ConcurrentDictionary<Guid,GameUser> GameUsers();
    GameUser? GetUserByServerPlayer(IPlayer serverPlayer);
    void SendServerBattleEnded(Guid gameId, string reason);

    void SendListOfGamesUpdated(Guid[] gameIds);

    // GameUser? FindGameUserByConnectionId(string connectionId);
    void UpdatePlayersName(string battleId, string? player1Name, string? player2Name);
    void PlayerLeftGame(string gameConnectionId);
    event Action<GameUser?> OnPlayerLeftGame;
    void TryToJoinGame(Guid gameIdGuid, Guid userIdGuid, string connectionId);
    void PlayerSentCommand(Guid gameIdGuid, Guid userIdGuid, IClientCommand command);
    void ClientConnectedToLobby(string contextConnectionId);
    void ClientDisconnectedFromLobby(string contextConnectionId);
    void UserSentMessage(string contextConnectionId, string userName, string message);
    void ChangeUserName(string contextConnectionId, string newName);
    void UserIsReadyToPlay(string contextConnectionId, string userName, bool ready);
    event Action<GameUser>? OnUserConnectedToLobby;
    event Action<GameUser, GameUser> OnUsersReadyToPlay;
    event Action<Guid, GameUser>? OnTryToJoinGame;
    public event Action<Guid, GameUser, IClientCommand>? OnPlayerSentCommand;

    void UserJoinedGame(GameUser user, Guid gameId, string result);
    void SendUserGoToGame(ClientBattle battle, GameUser user);
    void SendPlayerAction(GameUser user, IServerAction action);
}