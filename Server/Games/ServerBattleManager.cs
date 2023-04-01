using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Server.Games;

public class ServerBattleManager : IServerBattleManager
{
    
    private static readonly ConcurrentDictionary<Guid, ServerBattle> Battles  = new ();


    public ConcurrentDictionary<Guid,ServerBattle> ServerBattles(){
        lock (Battles)
        {
            return Battles;
        }
    }
    public ServerBattle AllocateServerBattle(){
        lock (Battles)
        {
            var teamManager = new TeamManager();
            var battle = new ServerBattle(teamManager);
            Battles.TryAdd(battle.GameId, battle);
            return battle;
        }
    }

    public void EndServerBattle(Guid gameId, string reason){
        lock (Battles){
            Battles.TryRemove(gameId, out var battle);
            battle?.EndGame(reason);
        }
    } 


    public ServerBattle? FindServerBattleById(Guid gameId){
        lock (Battles)
        {
            Battles.TryGetValue(gameId, out var result);
            return result;
        }
    }


}