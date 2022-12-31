using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Domain.Entities;



public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}
    public static ConcurrentDictionary<string, HubGameClient> LobbyUsers = new ConcurrentDictionary<string, HubGameClient>();

    public override Task OnConnectedAsync()
    {
        ClientsConnected++;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();
        
        var connectionId = Context.ConnectionId;
        // Console.WriteLine($"User connected with ID: {connectionId}");
        // var userId = Context?.User?.Identity?.Name; // any desired user id
        lock(LobbyUsers){
            // LobbyUsers.TryAdd(connectionId, new HubGameClient() { ConnectionId = connectionId });
            LobbyUsers.TryAdd(connectionId, new HubGameClient() {});
        }
        Clients.All.SendAsync("ClientsList",LobbyUsers).GetAwaiter().GetResult();
        
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

    public async Task UserIsReadyToPlay(bool ready){
        var connectionId = Context.ConnectionId;
        lock(LobbyUsers){
            HubGameClient? client;
            LobbyUsers.TryGetValue(connectionId, out client);
            if (client != null){
                client.ReadyToPlay = ready;
            }
        }
        await Clients.All.SendAsync("ClientsList",LobbyUsers);
    }


}