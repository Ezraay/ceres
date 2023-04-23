using Ceres.Core.Networking.Messages;
using Ceres.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Ceres.Server.Hubs;

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

    public void SendMessage(ClientSendMessageNetworkMessage msg)
    {
        if (string.IsNullOrEmpty(msg.UserName) || string.IsNullOrEmpty(msg.Message))
            return;
        networkService.UserSentMessage(Context.ConnectionId, msg.UserName, msg.Message);
    }

    public void ChangeUserName(ClientChangeUserNameNetworkMessage msg){
        if (string.IsNullOrEmpty(msg.NewName))
            return;
        networkService.ChangeUserName(Context.ConnectionId, msg.NewName);
    }

    public void UserIsReadyToPlay(ClientReadyToPlayNetworkMessage msg){
        if (string.IsNullOrEmpty(msg.UserName))
            return;
        networkService.UserIsReadyToPlay(Context.ConnectionId, msg.UserName, msg.Ready);
    }


    

}