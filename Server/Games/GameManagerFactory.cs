using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

public enum EndGameManagerReasons
{
    OnePlayerLeft
}

public class GameManagerFactory{
    
    private static readonly ConcurrentDictionary<Guid, GameManager> _games =
                new ConcurrentDictionary<Guid, GameManager>();
    private readonly IHubContext<LobbyHub> _loobyHub;

    public GameManagerFactory(IHubContext<LobbyHub>  loobyHub){
        _loobyHub = loobyHub;
    }
    public ConcurrentDictionary<Guid,GameManager> Games(){
        return _games;
    }
    public GameManager GetGameManager(){
        lock (_games){
            var game = new Lazy<GameManager>().Value;
            var gameAdded = _games.TryAdd(game.GameId, game);
            if (gameAdded){
                _loobyHub.Clients.All.SendAsync("GamesList",_games).GetAwaiter().GetResult();
            }
            return game;
        }
    }

    public void EndGameManager(Guid gameId, EndGameManagerReasons reason){
        lock (_games){
            _games.TryRemove(gameId, out var manager);
            if (manager != null){
                manager?.EndGame(reason);
                _loobyHub.Clients.All.SendAsync("GamesList",_games).GetAwaiter().GetResult();
            }
        }
    } 

    public GameManager? GetGameManagerById(Guid gameId){
        GameManager? result;
        _games.TryGetValue(gameId, out result);
        return result;

    }

}