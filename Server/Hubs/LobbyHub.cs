using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Entities;



public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}
    public static ConcurrentDictionary<string, HubGameClient> MyUsers = new ConcurrentDictionary<string, HubGameClient>();

    public override Task OnConnectedAsync()
    {
        ClientsConnected++;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();
        
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"User connected with ID: {connectionId}");
        // var userId = Context?.User?.Identity?.Name; // any desired user id
        lock(MyUsers){
            MyUsers.TryAdd(connectionId, new HubGameClient() { ConnectionId = connectionId });
        }
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ClientsConnected--;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();

        Console.WriteLine($"User disconnected with ID: {Context.ConnectionId}");
        lock(MyUsers){
            HubGameClient? garbage;
            MyUsers.TryRemove(Context.ConnectionId, out garbage);
        }
        
        return base.OnDisconnectedAsync(exception);
    }

    // public async Task FindGame(){
    //     Console.WriteLine($"FindGame called. Clients connected = {ClientsConnected}");
    // }
}