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

    // }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        networkService.OnPlayerLeftGame(Context.ConnectionId);
        
        return base.OnDisconnectedAsync(exception);
    }
    
    public string JoinGame(string gameId, string userId)
    {
        if (Guid.TryParse(gameId, out var GameIdGuid) && Guid.TryParse(userId, out var UserIdGuid))
        {
            return networkService.JoinGame(GameIdGuid, UserIdGuid);
        }
        return JoinGameResults.NoGameFound;
    }



    public void PlayerSentCommand(string gameId, string userId, IClientCommand command)
    {
        if (Guid.TryParse(gameId, out var GameIdGuid) && Guid.TryParse(userId, out var UserIdGuid))
        {
            networkService.OnPlayerSentCommand(GameIdGuid, UserIdGuid, command);
        }
    }


}