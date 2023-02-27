using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Entities;
using Ceres.Server.Services;

public class LobbyHub : Hub
{
    public static int ClientsConnected { get; set;}
    
    
    private readonly ISignalRService networkService;

    public LobbyHub(ISignalRService networkService)
    {
        this.networkService = networkService;
    }

    public override Task OnConnectedAsync()
    {
        networkService.ClientConnectedToLobby(Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        networkService.ClientDisconnectedFromLobby(Context.ConnectionId);
        
        return base.OnDisconnectedAsync(exception);
    }

    public void SendMessage(string userName, string message)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(message))
            return;
        networkService.UserSentMessage(Context.ConnectionId, userName, message);
    }

    public void ChangeUserName(string newName){
        if (string.IsNullOrEmpty(newName))
            return;
        networkService.ChangeUserName(Context.ConnectionId, newName);
    }

    public void UserIsReadyToPlay(string userName, bool ready){
        if (string.IsNullOrEmpty(userName))
            return;
        networkService.UserIsReadyToPlay(Context.ConnectionId, userName, ready);
    }


    

}