using Microsoft.AspNetCore.SignalR;



public class LobbyHub : Hub
{
    public int ClientsConnected { get; set;}

    public async Task ClientConnected(){
        ClientsConnected++;
        await Clients.All.SendAsync("ClientsOnServer",ClientsConnected);
    }
}