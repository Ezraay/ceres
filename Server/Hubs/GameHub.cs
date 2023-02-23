using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;
using Ceres.Server.Services;
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    private readonly ServerBattleFactory _serverBattleFactory;
    private readonly CardDatabaseLoader _cardDatabaseLoader;
    private readonly CardDeckLoader _cardDeckLoader;

    public GameHub(ServerBattleFactory gameManagerFactory, CardDatabaseLoader cardDatabaseLoader, CardDeckLoader cardDeckLoader)
    {
        _serverBattleFactory = gameManagerFactory;
        _cardDatabaseLoader = cardDatabaseLoader;
        _cardDeckLoader = cardDeckLoader;
    }

    // public override Task OnConnectedAsync()
    // {

    // }

        public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.ConnectionId;
        (GameUser? user, bool isplayer1) res = _serverBattleFactory.FindGameUserByConnectionId(userId);
        if (res.user != null){
            
            Guid game = res.user.GameId;
            Console.WriteLine($"User {res.user.UserName} disconnected form the game {game}");
            if (res.isplayer1){
                _serverBattleFactory.EndServerBattle(game, EndServerBattleReasons.Player1Left);
            } else {
                _serverBattleFactory.EndServerBattle(game, EndServerBattleReasons.Player2Left);
            }
        }

        // Clients.All.SendAsync("ClientsList",LobbyUsers).GetAwaiter().GetResult();
        // Clients.All.SendAsync("GamesList",_serverBattleFactory.Games()).GetAwaiter().GetResult();
        
        return base.OnDisconnectedAsync(exception);
    }


    public async Task<string> JoinGame(string gameId, string userId)
    {
        if (Guid.TryParse(gameId, out var GameIdGuid))
        {
            ServerBattle? serverBattle = _serverBattleFactory.FindServerBattleById(GameIdGuid);
            if (serverBattle == null)
                return JoinGameResults.NoGameFound;

            await Groups.AddToGroupAsync(Context.ConnectionId,
                serverBattle.GameId.ToString()); // Adding player or spectator to the Game group
            if (Guid.TryParse(userId, out var UserIdGuid))
            {
                if ((serverBattle.Player1 as GameUser)?.UserId == UserIdGuid)
                {
                    var user1 = (GameUser) serverBattle.Player1;
                    user1.ConnectionId = Context.ConnectionId;
                    user1.LoadDeck(_cardDeckLoader.Deck);
                    await UpdatePlayersName(serverBattle);
                    return JoinGameResults.JoinedAsPlayer1;
                }

                if ((serverBattle.Player2 as GameUser)?.UserId == UserIdGuid)
                {
                    var user2 = (GameUser) serverBattle.Player2;
                    user2.ConnectionId = Context.ConnectionId;
                    user2.LoadDeck(_cardDeckLoader.Deck);
                    await UpdatePlayersName(serverBattle);
                    return JoinGameResults.JoinedAsPlayer2;
                }

                // spectators
                // await UpdatePlayersName(serverBattle);
                // return JoinGameResults.JoinedAsSpectator;
            }

            // spectators
            await UpdatePlayersName(serverBattle);
            return JoinGameResults.JoinedAsSpectator;
        }

        return JoinGameResults.NoGameFound;
    }

    public async Task UpdatePlayersName(ServerBattle serverBattle)
    {
        await Clients.Group(serverBattle.GameId.ToString()).SendAsync("UpdatePlayersName",
            serverBattle.Player1?.UserName, serverBattle.Player2?.UserName);
    }

    public void PlayerSentCommand(string gameId, string userId, TestDrawCommand command)
    {
        if (Guid.TryParse(gameId, out var GameIdGuid) && Guid.TryParse(userId, out var UserIdGuid))
        {
            ServerBattle? serverBattle = _serverBattleFactory.FindServerBattleById(GameIdGuid);
            if (serverBattle != null) serverBattle.Execute(command, serverBattle.GetGameUserById(UserIdGuid));

        }
    }


}