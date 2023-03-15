using System.Collections.Concurrent;
using Ceres.Core.Entities;

namespace Ceres.Server.Games;



public class GameUsers 
{
    private static readonly ConcurrentDictionary<Guid, GameUser> gameUsers = new();
    
    
    public ConcurrentDictionary<Guid,GameUser> GetUsers()
    {
        lock (gameUsers)
        {
            return gameUsers;
        }
    }

    public GameUser? GetUserById(Guid userId)
    {
        lock (gameUsers)
        {
            gameUsers.TryGetValue(userId, out var user);
            return user;
        }
    }
    
    public void GetUserByLobbyConnectionId(string lobbyConnectionId, out GameUser? user)
    {
        user = null;
        lock (gameUsers)
        {
            var u =  gameUsers.Values
                .FirstOrDefault(user => user.LobbyConnectionId == lobbyConnectionId);
            user = u;
        }
    }
    public void GetUserByGameConnectionId(string gameConnectionId, out GameUser? user)
    {
        user = null;
        lock (gameUsers)
        {
            var u =  gameUsers.Values
                .FirstOrDefault(user => user.GameConnectionId == gameConnectionId);
            user = u;
        }
    }

    public bool AddUser(GameUser gameUser)
    {
        lock (gameUsers)
        {
            return gameUsers.TryAdd(gameUser.UserId, gameUser);
        }
    }
    
    public  GameUser? UpdateUserGameConnectionId(Guid userId, string gameConnectionId){
        lock (gameUsers)
        {
            var gameUser = gameUsers.Values
                .FirstOrDefault(u => u.UserId == userId);
            if (gameUser != null){
                gameUser.GameConnectionId = gameConnectionId;
            }
            return gameUser;
        }
    }
    
    public GameUser? UpdateUserLobbyConnectionId(Guid userId, string lobbyConnectionId){
        lock (gameUsers)
        {
            var gameUser = gameUsers.Values
                .FirstOrDefault(u => u.UserId == userId);
            if (gameUser != null){
                gameUser.LobbyConnectionId = lobbyConnectionId;
            }
            return gameUser;
        }
    }
    
}