using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;
using Ceres.Core.Networking.Messages;
using Ceres.Server.Services;
using Microsoft.AspNetCore.SignalR;

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
        // networkService.OnPlayerLeftGame.Invoke(Context.ConnectionId);
        
        return base.OnDisconnectedAsync(exception);
    }
    
    public void JoinGame(string gameId, string userId)
    {
        if (Guid.TryParse(gameId, out var gameIdGuid) && Guid.TryParse(userId, out var userIdGuid))
        {
             networkService.TryToJoinGame(gameIdGuid, userIdGuid, Context.ConnectionId);
        }
        // return JoinGameResults.NoGameFound;
    }



    public void PlayerSentCommand(string gameId, string userId, IClientCommand command)
    {
        if (Guid.TryParse(gameId, out var GameIdGuid) && Guid.TryParse(userId, out var UserIdGuid))
        {
            networkService.PlayerSentCommand(GameIdGuid, UserIdGuid, command);
        }
    }


}