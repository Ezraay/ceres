using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Enums;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Newtonsoft.Json;
using Ceres.Server.Services;

public class ServerBattleManager : IServerBattleManager
{
    
    private static readonly ConcurrentDictionary<Guid, ServerBattle> battles  = new ();
    private readonly CardDeckLoader cardDeckLoader;

    // private readonly ISignalRService networkService;

    // private readonly IBattleService battleService;

    public ServerBattleManager(  CardDeckLoader cardDeckLoader){
        this.cardDeckLoader = cardDeckLoader;
        // this.networkService = networkService;
        // this.battleService = battleService;
    }



    // private (ServerPlayer? player, bool isPlayer1) FindServerPlayer(GameUser user){
    //     var battles = ServerBattles();
    //     lock (battles)
    //     {
    //         var serverPlayers = battles.Values
    //             .SelectMany( battle => new[] {(battle.Player1, true), (battle.Player2, false)})
    //             .Where(pair => pair.Item1 is ServerPlayer && (pair.Item1 as ServerPlayer) == user.ServerPlayer)
    //             .ToList();
    //
    //         if (!serverPlayers.Any()){
    //             return (null, false);
    //         }
    //
    //         return serverPlayers[0];
    //     }   
    // }

    // public ServerPlayer? FindServerPlayer(Guid battleId, Guid userId){
    //     var serverBattle =FindServerBattleById(battleId);
    //     if (serverBattle != null){
    //         return FindServerPlayer(serverBattle, userId); 
    //     } else 
    //        return null;
    // }

    // public ServerPlayer? FindServerPlayer(GameUser gameUser)
    // {
    //     return FindServerPlayer(gameUser.GameId, gameUser.UserId);
    // }

    // public ServerPlayer? FindServerPlayer(ServerBattle battle, Guid userId){
        // return FindServerPlayer(battle.GameId, userId);
    // }




    public ConcurrentDictionary<Guid,ServerBattle> ServerBattles(){
        lock (battles)
        {
            return battles;
        }
    }
    public ServerBattle AllocateServerBattle(Guid gameId){
        lock (battles)
        {
            var teamManager = new TeamManager();
            var battle = new ServerBattle(teamManager);
            // battle.OnPlayerAction += battleService.PlayerAction;
            var gameAdded = battles.TryAdd(gameId, battle);
            if (gameAdded){
                // battleService.SendListOfGamesUpdated(battles);
            }
            return battle;
        }
    }

    public void EndServerBattle(Guid gameId, string reason){
        lock (battles){
            battles.TryRemove(gameId, out var battle);
            if (battle != null){
                battle.EndGame(reason);
            }
        }
    } 

//    public string JoinBattle(Guid battleId, Guid userId, string connectionId)
//     {
//         ServerBattle? serverBattle =FindServerBattleById(battleId);
//         if (serverBattle == null)
//             return JoinGameResults.NoGameFound;

//         var serverPlayer = FindServerPlayer(battleId, userId);
//         if (serverPlayer == null)
//             return JoinGameResults.NoGameFound;

//         if (serverPlayer.Equals(serverBattle.Player1)){
//             serverBattle.Player1.LoadDeck(cardDeckLoader.Deck);
//             // UpdatePlayersName(serverBattle);  
//             return JoinGameResults.JoinedAsPlayer1;
//         }
        
//         if (serverPlayer.Equals(serverBattle.Player2)){
//             serverBattle.Player2.LoadDeck(cardDeckLoader.Deck);
//             // UpdatePlayersName(serverBattle);  
//             return JoinGameResults.JoinedAsPlayer2;
//         }

//         // UpdatePlayersName(serverBattle);  
//         return JoinGameResults.JoinedAsSpectator;
//     }


    public ServerBattle? FindServerBattleById(Guid gameId){
        lock (battles)
        {
            battles.TryGetValue(gameId, out var result);
            return result;
        }
    }

    public void StopBattle(Guid battleId, string reason)
    {
        
        EndServerBattle(battleId, reason);
        // battleService.SendServerBattleEnded(battleId, reason);
        // var games = ServerBattles().Keys.Select(key => key.ToString()).ToArray();
        // battleService.SendListOfGamesUpdated(games);
    }

    // public void PlayerLeftGame(string connectionId)
    // {
    //     GameUser? user = battleService.FindGameUserByConnectionId(connectionId);
    //     if (user != null){
    //         Guid game = user.GameId;
    //         Console.WriteLine($"User {user.UserName} disconnected form the game {game}");
    //         var res = FindServerPlayer(user);
    //         if (res.player != null && res.isPlayer1){
    //             StopBattle(game, EndServerBattleReasons.Player1Left);
    //         }
    //         if (res.player != null && !res.isPlayer1){
    //             StopBattle(game, EndServerBattleReasons.Player2Left);
    //         }
    //     }


    // }

    // public (GameUser?, bool) FindGameUserByConnectionId(string userConnectionId) {
    //      lock (battles){

    //         var gameUsers = battles.Values
    //             .SelectMany(battle => new[] { (battle.Player1, true), (battle.Player2, false) })
    //             .Where(pair => pair.Item1 is GameUser && (pair.Item1 as GameUser)?.ConnectionId == userConnectionId)
    //             .ToList();

    //     if (gameUsers.Count == 0)
    //     {
    //         return (null, false);
    //     }

    //     return ((GameUser?, bool))gameUsers[0];  

    //      }
    // }



}