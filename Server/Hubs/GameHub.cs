using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Enums;
using Ceres.Core.Entities;
using Ceres.Core.BattleSystem;

public class GameHub : Hub
{

    private readonly ServerBattleFactory _serverBattleFactory;

    public GameHub(ServerBattleFactory gameManagerFactory){
        _serverBattleFactory = gameManagerFactory;

    }

    // public override Task OnConnectedAsync()
    // {
        
    // }



    public async Task<string> JoinGame(string gameId, string userId){
        if (Guid.TryParse(gameId, out var GameIdGuid)){
                ServerBattle? serverBattle = _serverBattleFactory.GetServerBattleById(GameIdGuid);
                if (serverBattle == null)
                    return JoinGameResults.NoGameFound;

                await Groups.AddToGroupAsync(Context.ConnectionId,serverBattle.GameId.ToString());   // Adding player or spectator to the Game group
                if (Guid.TryParse(userId, out var UserIdGuid)){
                    if ((serverBattle.Player1 as GameUser)?.UserId == UserIdGuid){
                        var user1 = (GameUser)serverBattle.Player1;
                        user1.ConnectionId = Context.ConnectionId;
                        await UpdatePlayersName(serverBattle);
                        return JoinGameResults.JoinedAsPlayer1;
                    } else{
                        if ((serverBattle.Player2 as GameUser)?.UserId == UserIdGuid){
                            var user2 = (GameUser)serverBattle.Player2;
                            user2.ConnectionId = Context.ConnectionId;
                            await UpdatePlayersName(serverBattle);
                            return JoinGameResults.JoinedAsPlayer2;
                        }
                    } 
                    // spectators
                    await UpdatePlayersName(serverBattle);
                    return JoinGameResults.JoinedAsSpectator;  
                } else {
                    // spectators
                    await UpdatePlayersName(serverBattle);
                    return JoinGameResults.JoinedAsSpectator;  
                }
        } else
            return JoinGameResults.NoGameFound;
    }

    public async Task UpdatePlayersName(ServerBattle serverBattle){
        await Clients.Group(serverBattle.GameId.ToString()).SendAsync("UpdatePlayersName", serverBattle.Player1?.UserName, serverBattle.Player2?.UserName );
    }



    

}