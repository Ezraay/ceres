using Microsoft.AspNetCore.SignalR;



public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}

    public override Task OnConnectedAsync()
    {
        ClientsConnected++;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ClientsConnected--;
        Clients.All.SendAsync("ClientsOnServer",ClientsConnected).GetAwaiter().GetResult();
        return base.OnDisconnectedAsync(exception);
    }

    // public async Task FindGame(){
    //     Console.WriteLine($"FindGame called. Clients connected = {ClientsConnected}");
    // }
}