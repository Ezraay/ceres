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
    public event Action<Guid, GameUser>? OnTryToJoinBattle;
    public event Action<Guid, GameUser, IClientCommand>? OnPlayerSentCommand;


    public SignalRService( IHubContext<GameHub> gameHub, IHubContext<LobbyHub> lobbyHub)
    {
        this.gameHub = gameHub;
        this.lobbyHub = lobbyHub;

        this.gameUsers = new GameUsers();
    }



    private void SendHubMessage(IHubContext<Hub> context, INetworkMessage message, params string[] connectionIds)
    {
        context.Clients.Clients(connectionIds).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
        
        // context.Clients.Client(connectionId).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
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
            .Where(pair => !string.IsNullOrEmpty(pair.Value.LobbyConnectionId) )
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
                    OnUsersReadyToPlay?.Invoke(firstPlayer, user);
                }
            }
        }
    }
#endregion
#region Games

    public void TryToJoinGame(Guid gameId, Guid userId, string gameConnectionId){
        var gameUser = gameUsers.UpdateUserGameConnectionId(userId, gameConnectionId);

        if (gameUser == null) return;

        gameHub.Groups.AddToGroupAsync(gameConnectionId, gameId.ToString()).GetAwaiter().GetResult();
        
        OnTryToJoinBattle?.Invoke(gameId, gameUser);
        SendLobbyClientListMessage();
    }

    public void UpdatePlayersName(string battleId, List<BattleTeam> allies, List<BattleTeam> enemies)
    {
        var msg = new UpdatePlayersNameMessage(){Allies = allies, Enemies = enemies};
        SendHubGroupMessage(gameHub,battleId, msg);

    }


    public void SendPlayerAction(GameUser user, IServerAction action) {
        // var gameId = user. GameId;
        // Console.WriteLine("Sending action: " + action + " to: " + connectionId);
        var msg = new ServerActionMessage()
        {
            Action = action
        };
        SendHubMessage(gameHub, msg, user.GameConnectionId);
    }

    // public void SendServerBattleEnded(Guid gameId, string reason){
    //     var msg = new GameEndedMessage() {GameId = gameId.ToString(), Reason = reason};
    //     SendHubGroupMessage(gameHub, msg.GameId, msg);
    // }

    public void SendServerBattleLost(GameUser[] losers)
    {
        var msg = new GameEndedMessage() {Reason = EndBattleReason.YouLost};
        SendHubMessage(gameHub, msg, losers.Select(x => x.GameConnectionId).ToArray());   
    }

    public void SendServerBattleWon(GameUser[] winners)
    {
        var msg = new GameEndedMessage() {Reason = EndBattleReason.YouWon};
        SendHubMessage(gameHub, msg, winners.Select(x => x.GameConnectionId).ToArray());
    }

    public void PlayerLeftGame(string gameConnectionId)
    {
        gameUsers.GetUserByGameConnectionId(gameConnectionId, out var user);
        if (user == null) return;
        OnPlayerLeftGame?.Invoke(user);
    }

    public void SendUserGoToGame(ClientBattle battle, GameUser user)
    {
        if (user.LobbyConnectionId == "") return;

        var playerId = user.ServerPlayer?.Id ?? Guid.Empty;
        var msg = new GoToGameMessage() {
            GameId = user.GameId, 
            UserId = user.UserId, 
            ClientBattle = battle,
            PlayerId = playerId};
        // var msg = new GoToGameMessage() {GameId = user.GameId, UserId = user.UserId, ClientBattle = battle, PlayerId = playerId};

        SendHubMessage(lobbyHub, msg, user.LobbyConnectionId);
    }

    public void PlayerSentCommand(Guid gameId, Guid userId, IClientCommand command){
        var gameUser = gameUsers.GetUserById(userId);

        if ( gameUser != null) {
            OnPlayerSentCommand?.Invoke(gameId, gameUser, command);
        }
    }

    // public void UserJoinedGame(GameUser user, Guid gameId, string result)
    // {
    //     if (result == JoinGameResults.NoGameFound) return;
    //     
    //     // Adding player or spectator to the Game group
    //     gameHub.Groups.AddToGroupAsync(user.GameConnectionId, gameId.ToString()).GetAwaiter().GetResult();
    //     
    //     var msg = new JoinedGame() { GameJoiningResult = result };
    //     SendHubMessage(gameHub, msg, user.GameConnectionId);
    // }
    
    #endregion
}


