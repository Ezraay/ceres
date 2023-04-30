using Ceres.Core.BattleSystem;
using Ceres.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Ceres.Server.Hubs;

public class GameHub : Hub
{
    private readonly ISignalRService networkService;

    public GameHub(ISignalRService networkService)
    {
        this.networkService = networkService;
    }

    // public override Task OnConnectedAsync()
    // {
    //     networkService.ClientConnectedToGame(Context.ConnectionId);
    //
    //     return base.OnConnectedAsync();
    // }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        networkService.PlayerLeftGame(Context.ConnectionId);
        
        return base.OnDisconnectedAsync(exception);
    }
    
    public void JoinGame(string gameId, string userId)
    {
        if (Guid.TryParse(gameId, out var gameIdGuid) && Guid.TryParse(userId, out var userIdGuid))
        {
            networkService.TryToJoinGame(gameIdGuid, userIdGuid, Context.ConnectionId);
        }
    }



    public void PlayerSentCommand(string gameId, string userId, ClientCommand command)
    {
        if (Guid.TryParse(gameId, out var gameIdGuid) && Guid.TryParse(userId, out var userIdGuid))
        {
            networkService.PlayerSentCommand(gameIdGuid, userIdGuid, command);
        }
    }


}