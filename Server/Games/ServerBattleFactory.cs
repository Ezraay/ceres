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
    public ConcurrentDictionary<Guid,ServerBattle> ServerBattles(){
        return _battles;
    }
    public ServerBattle GetServerBattle(){
        lock (_battles){
            // var battle = new Lazy<ServerBattle>().Value;
            // var battle = new ServerBattle(new GameUser(), new GameUser(), true);
            var battle = new ServerBattle(null, null, true);
            battle.OnPlayerAction += SendPlayerAction;
            var gameAdded = _battles.TryAdd(battle.GameId, battle);
            if (gameAdded){
                _lobbyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
            return battle;
        }
    }

    public void EndServerBattle(Guid gameId, string reason){
        lock (_battles){
            _battles.TryRemove(gameId, out var battle);
            if (battle != null){
                battle.EndGame(reason);
                _gameHub.Clients.Group(battle.GameId.ToString()).SendAsync("GameEnded", reason).GetAwaiter().GetResult();
                _lobbyHub.Clients.All.SendAsync("GamesList",_battles).GetAwaiter().GetResult();
            }
            battle = null;
        }
    } 

    public ServerBattle? FindServerBattleById(Guid gameId){
        ServerBattle? result;
        _battles.TryGetValue(gameId, out result);
        return result;

    }

    public (GameUser?, bool) FindGameUserByConnectionId(string userConnectionId) {
         lock (_battles){
            
            var gameUsers = _battles.Values
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

    public async void SendPlayerAction(ServerPlayer player, IServerAction action){
        var gameId = ((GameUser)player).GameId;
        var connectionId = ((GameUser)player).ConnectionId;
        await _gameHub.Clients.Client(connectionId).SendAsync("ServerAction",action);
        // await _gameHub.Clients.Group(gameId.ToString()).SendAsync("ServerAction",action);
    }

}