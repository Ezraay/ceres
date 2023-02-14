using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Ceres.Core.Enums;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Newtonsoft.Json;

public class ServerBattleFactory{
    
    private static readonly ConcurrentDictionary<Guid, ServerBattle> _battles = new ();
    private readonly IHubContext<LobbyHub> _lobbyHub;
    private readonly IHubContext<GameHub> _gameHub;

    public ServerBattleFactory(IHubContext<LobbyHub>  lobbyHub, IHubContext<GameHub> gameHub){
        _lobbyHub = lobbyHub;
        _gameHub = gameHub;
    }
    public ConcurrentDictionary<Guid,ServerBattle> Games(){
        return _battles;
    }
    public ServerBattle GetServerBattle(){
        lock (_battles){
            // var battle = new Lazy<ServerBattle>().Value;
            var battle = new ServerBattle(new GameUser(), new GameUser(), true);
            battle.OnPlayerAction += SendPlayerAction;
            var gameAdded = _battles.TryAdd(battle.GameId, battle);
            if (gameAdded){
                _lobbyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
            return battle;
        }
    }

    public void EndGameManager(Guid gameId, EndServerBattleReasons reason){
        lock (_battles){
            _battles.TryRemove(gameId, out var battle);
            if (battle != null){
                battle?.EndGame(reason);
                _lobbyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
        }
    } 

    public ServerBattle? GetServerBattleById(Guid gameId){
        ServerBattle? result;
        _battles.TryGetValue(gameId, out result);
        return result;

    }

    public async void SendPlayerAction(ServerPlayer player, IServerAction action){
        var gameId = ((GameUser)player).GameId;
        await _gameHub.Clients.Group(gameId.ToString()).SendAsync("ServerAction",action);
    }

}