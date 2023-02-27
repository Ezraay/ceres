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

    public ServerBattleManager(CardDeckLoader cardDeckLoader){
        this.cardDeckLoader = cardDeckLoader;
        // this.networkService = networkService;
        // this.battleService = battleService;
    }
    public ConcurrentDictionary<Guid,ServerBattle> ServerBattles(){
        return battles;
    }
    public ServerBattle GetServerBattle(){
        lock (battles){
            var battle = new ServerBattle(null, null, true);
            // battle.OnPlayerAction += battleService.PlayerAction;
            var gameAdded = battles.TryAdd(battle.GameId, battle);
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


   public string JoinBattle(Guid battleId, ServerPlayer serverPlayer)
    {
        ServerBattle? serverBattle = FindServerBattleById(battleId);
        if (serverBattle == null)
            return JoinGameResults.NoGameFound;

        if (serverPlayer.Equals(serverBattle.Player1)){
            serverBattle.Player1.LoadDeck(cardDeckLoader.Deck);
            // UpdatePlayersName(serverBattle);  
            return JoinGameResults.JoinedAsPlayer1;
        }
        
        if (serverPlayer.Equals(serverBattle.Player2)){
            serverBattle.Player2.LoadDeck(cardDeckLoader.Deck);
            // UpdatePlayersName(serverBattle);  
            return JoinGameResults.JoinedAsPlayer2;
        }

        // UpdatePlayersName(serverBattle);  
        return JoinGameResults.JoinedAsSpectator;
    }

    public ServerBattle? FindServerBattleById(Guid gameId){
        battles.TryGetValue(gameId, out var result);
        return result;
    }

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