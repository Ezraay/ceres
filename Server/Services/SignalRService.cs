using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Ceres.Server.Services;
public class SignalRService :ISignalRService
{
    private readonly IHubContext<GameHub> gameHub;
    private readonly IHubContext<LobbyHub> lobbyHub;
    // private readonly IBattleService battleService;
    private readonly IServerBattleManager serverBattleManager;
    private static int _userNumber;

    // public event EventHandler? EventPlayerSentCommand;

    private static ConcurrentDictionary<string, GameUser> lobbyUsers = new ConcurrentDictionary<string, GameUser>();


    public SignalRService( IHubContext<GameHub> gameHub, IHubContext<LobbyHub> lobbyHub, IServerBattleManager serverBattleManager)
    {
        this.gameHub = gameHub;
        this.lobbyHub = lobbyHub;
        // this.battleService = battleService;
        this.serverBattleManager = serverBattleManager;
    }

    public ConcurrentDictionary<string,GameUser> LobbyUsers(){
        return lobbyUsers;
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


#region Lobby
    public void ClientConnectedToLobby(string connectionId){
        // Console.WriteLine($"User connected with ID: {connectionId}");
        lock(lobbyUsers){
            lobbyUsers.TryAdd(connectionId, new GameUser() {ConnectionId = connectionId, 
                UserName = $"Player{_userNumber}", UserId = Guid.NewGuid()});
        }
        _userNumber++;
        var msg = new ClientsListMessage(){LobbyUsers = lobbyUsers};
        SendHubAllMessage(lobbyHub,msg);
    }

    public void ClientDisconnectedFromLobby(string connectionId){
        // Console.WriteLine($"User disconnected with ID: {Context.ConnectionId}");
        lock(lobbyUsers){
            GameUser? garbage;
            lobbyUsers.TryRemove(connectionId, out garbage);
            garbage = null;
        }
        var msg = new ClientsListMessage(){LobbyUsers = lobbyUsers};
        SendHubAllMessage(lobbyHub,msg);
    }

    public void UserSentMessage(string connectionId, string userName, string message){
        ChangeUserName(connectionId, userName);
        var msg = new ReceiveMessageMessage() {UserName = userName, MessageText = message};
        SendHubAllMessage(lobbyHub,msg);
    }

    public void SendListOfGamesUpdated(string[] games){
        var msg = new UpdateGamesMessage() {GameNames = games};
        SendHubAllMessage(lobbyHub, msg);
    }
    public void ChangeUserName(string connectionId, string newName){
        lock(lobbyUsers){
            GameUser? client;
            lobbyUsers.TryGetValue(connectionId, out client);
            if (client != null && client.UserName != newName){
                client.UserName = newName;

                var msg = new ClientsListMessage(){LobbyUsers = lobbyUsers};
                SendHubAllMessage(lobbyHub,msg);
            }
        }
    }

    public void UserIsReadyToPlay(string connectionId, string userName, bool ready){
        ChangeUserName(connectionId, userName);
        lock(lobbyUsers){
            GameUser? client;
            lobbyUsers.TryGetValue(connectionId, out client);
            if (client != null){
                client.ReadyToPlay = ready;
            }
        }
        var msg = new ClientsListMessage(){LobbyUsers = lobbyUsers};
        SendHubAllMessage(lobbyHub,msg);

        TryAllocateServerBattle();
    }

    private void TryAllocateServerBattle(){
        GameUser? firstPlayer = null;
        lock(lobbyUsers){
            foreach (var keyValuePair in lobbyUsers)
            {
                var user = keyValuePair.Value;
                if (user == null) continue;
                if (user.GameId == Guid.Empty  && user.ReadyToPlay){
                    if (firstPlayer == null){
                        firstPlayer = user;
                    } else {
                        // var battle = battleService.AllocateServerBattle();
                        var battle = serverBattleManager.GetServerBattle();
                        
                        user.GameId = battle.GameId;
                        battle.Player2 = new ServerPlayer();
                        user.ServerPlayer = battle.Player2;

                        firstPlayer.GameId = battle.GameId;
                        battle.Player1 = new ServerPlayer();
                        firstPlayer.ServerPlayer = battle.Player1; 

                        if (user.ConnectionId != null){
                            var msg = new GoToGameMessage() {GameId = battle.GameId, UserId = user.UserId };
                            SendHubMessage(lobbyHub, user.ConnectionId, msg);
                        }
                        if (firstPlayer.ConnectionId != null){
                            var msg = new GoToGameMessage() {GameId = battle.GameId, UserId = firstPlayer.UserId };
                            SendHubMessage(lobbyHub, firstPlayer.ConnectionId, msg);
                        }
                    }
                }
            }
        }
    }
#endregion
#region Games

    public GameUser? FindGameUserByConnectionId(string connectionId){
        lock (lobbyUsers)
        {
            return lobbyUsers.Values
                .Where(u => u.ConnectionId == connectionId)
                .FirstOrDefault();
        }
    }

    public string JoinGame(Guid gameId, Guid userId){
        lock(lobbyUsers){
            if ( lobbyUsers.TryGetValue(userId.ToString(), out var gameUser) ){
                var res =  serverBattleManager.JoinBattle(gameId, gameUser.ServerPlayer);
                if (res != JoinGameResults.NoGameFound){
                    // Adding player or spectator to the Game group
                    gameHub.Groups.AddToGroupAsync(gameUser.ConnectionId, gameId.ToString()).GetAwaiter().GetResult();
                    return res;
                }

            }
        }
        
        return JoinGameResults.NoGameFound;
    }

    public void UpdatePlayersName(string battleId, string? player1Name, string? player2Name)
    {
        var msg = new UpdatePlayersNameMessage(){Player1Name = player1Name, Player2Name = player2Name};
        SendHubGroupMessage(gameHub,battleId, msg);

    }


    public void SendPlayerAction(GameUser user, IServerAction action){
        // var gameId = user. GameId;
        var connectionId = user.ConnectionId;
        // Console.WriteLine("Sending action: " + action + " to: " + connectionId);
        var msg = new ServerActionMessage() {Action = action};
        SendHubMessage(gameHub, connectionId, msg);
    }

    public void SendServerBattleEnded(Guid gameId, string reason){
        var msg = new GameEndedMessage() {GameId = gameId.ToString(), Reason = reason};
        SendHubGroupMessage(gameHub, msg.GameId, msg);
    }

    public void OnPlayerLeftGame(string connectionId)
    {
        // battleService.PlayerLeftGame(connectionId);
    }


    public void OnPlayerSentCommand(Guid gameId, Guid userId, IClientCommand command){
        // PlayerSentCommandMessage eventArgs = new PlayerSentCommandMessage(){GameId = gameId.ToString(), 
        //     UserId = userId.ToString(), Command = command};
        // EventPlayerSentCommand?.Invoke(this, eventArgs);

        // ServerBattle? serverBattle = serverBattleManager.FindServerBattleById(gameId);
        // if (serverBattle != null) serverBattle.Execute(command, serverBattle.GetGameUserById(userId));

    }

    // public void PlayerAction(ServerPlayer player, IServerAction action)
    // {
    //     var gameUser = FindGameUser(player);
    //     if (gameUser != null){
    //         _networkService.SendPlayerAction(gameUser, action);
    //     }
    // }

    // public void StartServerBattle()
    // {

    // }
       
#endregion
}