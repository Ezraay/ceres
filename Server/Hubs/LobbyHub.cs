using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Domain.Entities;



public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}
    public static ConcurrentDictionary<string, HubGameClient> LobbyUsers = new ConcurrentDictionary<string, HubGameClient>();

    private readonly GameManagerFactory _gameManagerFactory;
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
            LobbyUsers.TryAdd(connectionId, new HubGameClient() {LobbyConnectionId = connectionId});
        }
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
            HubGameClient? garbage;
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
        var connectionId = Context.ConnectionId;
        lock(LobbyUsers){
            HubGameClient? client;
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
            HubGameClient? client;
            LobbyUsers.TryGetValue(connectionId, out client);
            if (client != null){
                client.ReadyToPlay = ready;
            }
        }
        TryAllocateGameManager();
        await Clients.All.SendAsync("ClientsList",LobbyUsers);
    }


    private void TryAllocateGameManager(){
        HubGameClient? firstPlayer = null;
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
                        firstPlayer.GameId = manager.GameId;
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