using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;
using Ceres.Server.Services;

public class BattleService : IBattleService
{
    private readonly IServerBattleManager _battleManager;
    private readonly ISignalRService _networkService;
    // private readonly CardDeckLoader cardDeckLoader;

    public BattleService(IServerBattleManager battleManager, ISignalRService networkService) 
    {
        _battleManager = battleManager;
        _networkService = networkService;
        // this.cardDeckLoader = cardDeckLoader;
    }

    // public ServerBattle AllocateServerBattle()
    // {
    //     return _battleManager.GetServerBattle();
    // }
    
    private GameUser? FindGameUser(ServerPlayer serverPlayer){
        var lobbyUsers = _networkService.LobbyUsers();
        lock (lobbyUsers)
        {
            var gameUser = lobbyUsers.Values
                .FirstOrDefault(u => u.ServerPlayer == serverPlayer);

            return gameUser;
        }
    }

    private (ServerPlayer? player, bool isPlayer1) FindServerPlayer(GameUser user){
        var battles = _battleManager.ServerBattles();
        lock (battles)
        {
            var serverPlayers = battles.Values
                .SelectMany( battle => new[] {(battle.Player1, true), (battle.Player2, false)})
                .Where(pair => pair.Item1 is ServerPlayer && (pair.Item1 as ServerPlayer) == user.ServerPlayer)
                .ToList();

                if (!serverPlayers.Any()){
                    return (null, false);
                }

                return serverPlayers[0];
        }   
    }



    
    public void StartBattle(ServerBattle battle)
    {
        throw new NotImplementedException();
    }

    public void StopBattle(Guid battleId, string reason)
    {
        _battleManager.EndServerBattle(battleId, reason);
        _networkService.SendServerBattleEnded(battleId, reason);
        var games = _battleManager.ServerBattles().Keys.Select(key => key.ToString()).ToArray();
        _networkService.SendListOfGamesUpdated(games);
    }

     public void UpdatePlayersName(ServerBattle serverBattle){
        _networkService.UpdatePlayersName(serverBattle.GameId.ToString(), FindGameUser(serverBattle.Player1)?.UserName,
                                            FindGameUser(serverBattle.Player2)?.UserName);  
    }

    // public string JoinBattle(Guid battleId, GameUser gameUser)
    // {
    //     ServerBattle? serverBattle = _battleManager.FindServerBattleById(battleId);
    //     if (serverBattle == null)
    //         return JoinGameResults.NoGameFound;

    //     if (gameUser.ServerPlayer.Equals(serverBattle.Player1)){
    //         serverBattle.Player1.LoadDeck(cardDeckLoader.Deck);
    //         UpdatePlayersName(serverBattle);  
    //         return JoinGameResults.JoinedAsPlayer1;
    //     }
        
    //     if (gameUser.ServerPlayer.Equals(serverBattle.Player2)){
    //         serverBattle.Player2.LoadDeck(cardDeckLoader.Deck);
    //         UpdatePlayersName(serverBattle);  
    //         return JoinGameResults.JoinedAsPlayer2;
    //     }

    //     UpdatePlayersName(serverBattle);  
    //     return JoinGameResults.JoinedAsSpectator;
    // }

    public void PlayerLeftGame(string connectionId)
    {
        GameUser? user = _networkService.FindGameUserByConnectionId(connectionId);
        if (user != null){
            Guid game = user.GameId;
            Console.WriteLine($"User {user.UserName} disconnected form the game {game}");
            var res = FindServerPlayer(user);
            if (res.player != null && res.isPlayer1){
                StopBattle(game, EndServerBattleReasons.Player1Left);
            }
            if (res.player != null && !res.isPlayer1){
                StopBattle(game, EndServerBattleReasons.Player2Left);
            }
        }
    }



}
