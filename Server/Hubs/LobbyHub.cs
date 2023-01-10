using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Domain.Entities;



public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}
    public static ConcurrentDictionary<string, GameUser> LobbyUsers = new ConcurrentDictionary<string, GameUser>();

    private readonly GameManagerFactory _gameManagerFactory;
    private static int _userNumber;
    public LobbyHub(GameManagerFactory gameManagerFactory){
        _gameManagerFactory = gameManagerFactory;
    }

    public override Task OnConnectedAsync()
    {
        ClientsConnected++;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();
        
        var connectionId = Context.ConnectionId;
        // Console.WriteLine($"User connected with ID: {connectionId}");
        // var userId = Context?.User?.Identity?.Name; // any desired user id
        lock(LobbyUsers){
            LobbyUsers.TryAdd(connectionId, new GameUser() {LobbyConnectionId = connectionId, 
                UserName = $"Player{_userNumber}", UserId = Guid.NewGuid()});
        }
        _userNumber++;
        Clients.All.SendAsync("ClientsList",LobbyUsers).GetAwaiter().GetResult();
        Clients.All.SendAsync("GamesList",_gameManagerFactory.Games()).GetAwaiter().GetResult();
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ClientsConnected--;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();

        // Console.WriteLine($"User disconnected with ID: {Context.ConnectionId}");
        lock(LobbyUsers){
            GameUser? garbage;
            LobbyUsers.TryRemove(Context.ConnectionId, out garbage);
        }
        Clients.All.SendAsync("ClientsList",LobbyUsers).GetAwaiter().GetResult();
        Clients.All.SendAsync("GamesList",_gameManagerFactory.Games()).GetAwaiter().GetResult();
        
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        await ChangeUserName(user);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task ChangeUserName(string newName){
        if (string.IsNullOrEmpty(newName))
            return;
        var connectionId = Context.ConnectionId;
        lock(LobbyUsers){
            GameUser? client;
            LobbyUsers.TryGetValue(connectionId, out client);
            if (client != null){
                client.UserName = newName;
            }
        }
        await Clients.All.SendAsync("ClientsList",LobbyUsers);
    }

    public async Task UserIsReadyToPlay(string user, bool ready){
        await ChangeUserName(user);
        var connectionId = Context.ConnectionId;
        lock(LobbyUsers){
            GameUser? client;
            LobbyUsers.TryGetValue(connectionId, out client);
            if (client != null){
                client.ReadyToPlay = ready;
            }
        }
        TryAllocateGameManager();
        await Clients.All.SendAsync("ClientsList",LobbyUsers);
    }


    private void TryAllocateGameManager(){
        GameUser? firstPlayer = null;
        lock(LobbyUsers){
            foreach (var keyValuePair in LobbyUsers)
            {
                var user = keyValuePair.Value;
                if (user == null) continue;
                if (user.GameId == Guid.Empty  && user.ReadyToPlay){
                    if (firstPlayer == null){
                        firstPlayer = user;
                    } else {
                        var manager = _gameManagerFactory.GetGameManager();
                        user.GameId = manager.GameId;
                        manager.Player2 = user;
                        firstPlayer.GameId = manager.GameId;
                        manager.Player1 = firstPlayer;
                        if (user.LobbyConnectionId != null)
                            Clients.Client(user.LobbyConnectionId).SendAsync("GoToGame",manager.GameId).GetAwaiter().GetResult();
                        if (firstPlayer.LobbyConnectionId != null)
                            Clients.Client(firstPlayer.LobbyConnectionId).SendAsync("GoToGame",manager.GameId).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }

}