using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

namespace Ceres.Server.Services;

public interface ISignalRService
{
    GameUser? GetUserByServerPlayer(ServerPlayer serverPlayer);
    void SendServerBattleEnded(Guid gameId, string reason);
    void SendListOfGamesUpdated(string[] games);
    void UpdatePlayersName(string battleId, string? player1Name, string? player2Name);
    void PlayerLeftGame(string gameConnectionId);
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
    event Action<Guid, GameUser, IClientCommand>? OnPlayerSentCommand;
    event Action<GameUser> OnPlayerLeftGame;

    void UserJoinedGame(GameUser user, Guid gameId, string result);
    void SendUserGoToGame(GameUser user);
    void SendPlayerAction(GameUser user, IServerAction action);
}