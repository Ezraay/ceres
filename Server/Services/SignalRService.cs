using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Networking.Messages;
using Ceres.Server.Games;
using Ceres.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ceres.Server.Services;
public class SignalRService :ISignalRService
{
    private readonly IHubContext<GameHub> gameHub;
    private readonly IHubContext<LobbyHub> lobbyHub;
    private static int userNumber;

    private readonly GameUsers gameUsers;

    public event Action<GameUser>? OnUserConnectedToLobby;
    public event Action<GameUser>? OnPlayerLeftGame;
    public event Action<GameUser, GameUser>? OnUsersReadyToPlay;
    public event Action<Guid, GameUser>? OnTryToJoinGame;
    public event Action<Guid, GameUser, IClientCommand>? OnPlayerSentCommand;


    public SignalRService( IHubContext<GameHub> gameHub, IHubContext<LobbyHub> lobbyHub)
    {
        this.gameHub = gameHub;
        this.lobbyHub = lobbyHub;

        this.gameUsers = new GameUsers();
    }



    private void SendHubMessage(IHubContext<Hub> context, string connectionId, INetworkMessage message)
    {
        context.Clients.Client(connectionId).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }
    private void SendHubAllMessage(IHubContext<Hub> context, INetworkMessage message)
    {
        context.Clients.All.SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }
    private void SendHubGroupMessage(IHubContext<Hub> context, string groupId, INetworkMessage message)
    {
        context.Clients.Group(groupId).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }


    public GameUser? GetUserByServerPlayer(IPlayer serverPlayer)
    {
        return gameUsers.GetUsers().Values
            .FirstOrDefault(u => u.ServerPlayer == serverPlayer);
    }


#region Lobby

    private void SendLobbyClientListMessage()
    {
        var inLobbyOnlyGameUsers = gameUsers.GetUsers()
            .Where(pair => !string.IsNullOrEmpty(pair.Value.LobbyConnectionId) && pair.Value.GameId.Equals(Guid.Empty))
            .Select(p => p.Value ).ToArray();
        var msg = new ClientsListMessage(){LobbyUsers = inLobbyOnlyGameUsers};
        SendHubAllMessage(lobbyHub,msg);
    }

    public void ClientConnectedToLobby(string connectionId){
        // Console.WriteLine($"User connected with ID: {connectionId}");
        var userGuid = Guid.NewGuid();
        var user = new GameUser()
        {
            LobbyConnectionId = connectionId,
            UserName = $"Player{userNumber}", UserId = userGuid
        };
        gameUsers.AddUser(user);

        userNumber++;
        
        SendLobbyClientListMessage();
        
        OnUserConnectedToLobby?.Invoke(user);
    }


    public void ClientDisconnectedFromLobby(string connectionId){
        // Console.WriteLine($"User disconnected with ID: {Context.ConnectionId}");
            gameUsers.GetUserByLobbyConnectionId(connectionId, out var user);
            if (user == null) { return; }

            gameUsers.UpdateUserLobbyConnectionId(user.UserId, "");
            SendLobbyClientListMessage();
    }

    public void UserSentMessage(string connectionId, string userName, string message){
        ChangeUserName(connectionId, userName);
        var msg = new ReceiveMessageMessage() {UserName = userName, MessageText = message};
        SendHubAllMessage(lobbyHub,msg);
    }

    public void SendListOfGamesUpdated(Guid[] gameIds){
        var msg = new UpdateGamesMessage() {GameIds = gameIds};
        SendHubAllMessage(lobbyHub, msg);
    }
    public void ChangeUserName(string connectionId, string newName){
        lock(gameUsers){
            gameUsers.GetUserByLobbyConnectionId(connectionId, out var client);
            if (client == null || client.UserName == newName) return;
            client.UserName = newName;

            SendLobbyClientListMessage();
        }
    }

    public void UserIsReadyToPlay(string connectionId, string userName, bool ready){
        ChangeUserName(connectionId, userName);
        gameUsers.GetUserByLobbyConnectionId(connectionId, out var client);
        if (client != null)
        {
            client.ReadyToPlay = ready;
        }
        SendLobbyClientListMessage();
        
        if (ready){
            TryAllocateServerBattle();
        }

    }

    private void TryAllocateServerBattle(){
        GameUser? firstPlayer = null;
        foreach (var keyValuePair in gameUsers.GetUsers())
        {
            var user = keyValuePair.Value;
            if (user.GameId == Guid.Empty  && user.ReadyToPlay){
                if (firstPlayer == null){
                    firstPlayer = user;
                } else {
                    // var battle = battleService.AllocateServerBattle();
                    OnUsersReadyToPlay?.Invoke(firstPlayer, user);
                }
            }
        }
    }
#endregion
#region Games

    public void TryToJoinGame(Guid gameId, Guid userId, string gameConnectionId){
        var gameUser = gameUsers.UpdateUserGameConnectionId(userId, gameConnectionId);

        if ( gameUser != null) {
            OnTryToJoinGame?.Invoke(gameId, gameUser);
        }
    }

    public void UpdatePlayersName(string battleId, string? player1Name, string? player2Name)
    {
        var msg = new UpdatePlayersNameMessage(){Player1Name = player1Name, Player2Name = player2Name};
        SendHubGroupMessage(gameHub,battleId, msg);

    }


    public void SendPlayerAction(GameUser user, IServerAction action) {
        // var gameId = user. GameId;
        // Console.WriteLine("Sending action: " + action + " to: " + connectionId);
        var msg = new ServerActionMessage()
        {
            Action = action
        };
        SendHubMessage(gameHub, user.GameConnectionId, msg);
    }

    public void SendServerBattleEnded(Guid gameId, string reason){
        var msg = new GameEndedMessage() {GameId = gameId.ToString(), Reason = reason};
        SendHubGroupMessage(gameHub, msg.GameId, msg);
    }

    public void PlayerLeftGame(string gameConnectionId)
    {
        gameUsers.GetUserByGameConnectionId(gameConnectionId, out var user);
        if (user != null){
            OnPlayerLeftGame?.Invoke(user);
        }
    }

    public void SendUserGoToGame(ClientBattle battle, GameUser user)
    {
        if (user.LobbyConnectionId == null) return;

        var playerId = user.ServerPlayer?.Id ?? Guid.Empty;
        var msg = new GoToGameMessage() {
            GameId = user.GameId, 
            UserId = user.UserId, 
            ClientBattle = battle,
            PlayerId = playerId};
        // var msg = new GoToGameMessage() {GameId = user.GameId, UserId = user.UserId, ClientBattle = battle, PlayerId = playerId};

        SendHubMessage(lobbyHub, user.LobbyConnectionId, msg);
    }

    public void PlayerSentCommand(Guid gameId, Guid userId, IClientCommand command){
        var gameUser = gameUsers.GetUserById(userId);

        if ( gameUser != null) {
            OnPlayerSentCommand?.Invoke(gameId, gameUser, command);
        }
    }

    public void UserJoinedGame(GameUser user, Guid gameId, string result)
    {
        if (result == JoinGameResults.NoGameFound) return;
        
        // Adding player or spectator to the Game group
        gameHub.Groups.AddToGroupAsync(user.GameConnectionId, gameId.ToString()).GetAwaiter().GetResult();
        
        var msg = new JoinedGame() { GameJoiningResult = result };
        SendHubMessage(gameHub, user.GameConnectionId, msg);
    }
    
    #endregion
}


