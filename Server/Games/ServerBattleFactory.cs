using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Enums;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

public class ServerBattleFactory{
    
    private static readonly ConcurrentDictionary<Guid, ServerBattle> _battles =
                new ConcurrentDictionary<Guid, ServerBattle>();
    private readonly IHubContext<LobbyHub> _loobyHub;

    public ServerBattleFactory(IHubContext<LobbyHub>  loobyHub){
        _loobyHub = loobyHub;
    }
    public ConcurrentDictionary<Guid,ServerBattle> Games(){
        return _battles;
    }
    public ServerBattle GetServerBattle(){
        lock (_battles){
            // var battle = new Lazy<ServerBattle>().Value;
            var battle = new ServerBattle(new GameUser(), new GameUser());
            var gameAdded = _battles.TryAdd(battle.GameId, battle);
            if (gameAdded){
                _loobyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
            return battle;
        }
    }

    public void EndGameManager(Guid gameId, EndServerBattleReasons reason){
        lock (_battles){
            _battles.TryRemove(gameId, out var battle);
            if (battle != null){
                battle?.EndGame(reason);
                _loobyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
        }
    } 

    public ServerBattle? GetServerBattleById(Guid gameId){
        ServerBattle? result;
        _battles.TryGetValue(gameId, out result);
        return result;

    }

}