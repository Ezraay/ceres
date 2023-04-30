using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

namespace Ceres.Server.Services;

public interface ISignalRService
{
    // ConcurrentDictionary<Guid,GameUser> GameUsers();
    GameUser? GetUserByServerPlayer(IPlayer serverPlayer);
    // void SendServerBattleEnded(Guid gameId, string reason);
    void SendServerBattleWon(GameUser winner);
    void SendServerBattleLost(GameUser loser);
    void SendListOfGamesUpdated(Guid[] gameIds);

    // GameUser? FindGameUserByConnectionId(string connectionId);
    void UpdatePlayersName(string battleId, List<BattleTeam> allies, List<BattleTeam> enemies);
    void PlayerLeftGame(string gameConnectionId);
    void TryToJoinGame(Guid gameIdGuid, Guid userIdGuid, string connectionId);
    void PlayerSentCommand(Guid gameIdGuid, Guid userIdGuid, ClientCommand command);
    void ClientConnectedToLobby(string contextConnectionId);
    void ClientDisconnectedFromLobby(string contextConnectionId);
    void UserSentMessage(string contextConnectionId, string userName, string message);
    void ChangeUserName(string contextConnectionId, string newName);
    void UserIsReadyToPlay(string contextConnectionId, string userName, bool ready);
    event Action<GameUser>? OnUserConnectedToLobby;
    event Action<GameUser, GameUser> OnUsersReadyToPlay;
    event Action<Guid, GameUser>? OnTryToJoinBattle;
    event Action<Guid, GameUser, ClientCommand>? OnPlayerSentCommand;
    event Action<GameUser> OnPlayerLeftGame;

    // void UserJoinedGame(GameUser user, Guid gameId, string result);
    void SendUserGoToGame(ClientBattle battle, GameUser user);
    void SendPlayerAction(GameUser user, ServerAction action);
}