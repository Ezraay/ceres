using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Enums;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Newtonsoft.Json;
using Ceres.Server.Services;

public class ServerBattleManager{
    
    private static readonly ConcurrentDictionary<Guid, ServerBattle> battles = new ();
    private readonly NetworkService networkService;

    public ServerBattleManager(NetworkService networkService){
        this.networkService = networkService;
    }
    public ConcurrentDictionary<Guid,ServerBattle> ServerBattles(){
        return battles;
    }
    public ServerBattle GetServerBattle(){
        lock (battles){
            // var battle = new Lazy<ServerBattle>().Value;
            // var battle = new ServerBattle(new GameUser(), new GameUser(), true);
            var battle = new ServerBattle(null, null, true);
            battle.OnPlayerAction += networkService.SendPlayerAction;
            var gameAdded = battles.TryAdd(battle.GameId, battle);
            if (gameAdded){
                networkService.SendListOfGamesUpdated(battles);
            }
            return battle;
        }
    }

    public void EndServerBattle(Guid gameId, string reason){
        lock (battles){
            battles.TryRemove(gameId, out var battle);
            if (battle != null){
                battle.EndGame(reason);
                networkService.SendServerBattleEnded(gameId, reason);
                networkService.SendListOfGamesUpdated(battles);
            }
            battle = null;
        }
    } 

    public ServerBattle? FindServerBattleById(Guid gameId){
        ServerBattle? result;
        battles.TryGetValue(gameId, out result);
        return result;

    }

    public (GameUser?, bool) FindGameUserByConnectionId(string userConnectionId) {
         lock (battles){
            
            var gameUsers = battles.Values
                .SelectMany(battle => new[] { (battle.Player1, true), (battle.Player2, false) })
                .Where(pair => pair.Item1 is GameUser && (pair.Item1 as GameUser)?.ConnectionId == userConnectionId)
                .ToList();

        if (gameUsers.Count == 0)
        {
            return (null, false);
        }

        return ((GameUser?, bool))gameUsers[0];  

         }
    }



}