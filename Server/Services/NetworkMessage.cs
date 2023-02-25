using Ceres.Core.BattleSystem;

namespace Ceres.Server.Services;
public interface INetworkMessage 
{
    string MessageName {get;}
}

public class ServerActionMessage : INetworkMessage
{
    public string MessageName { get => "ServerAction"; }
    public IServerAction? Action { get; set;}
}

public class UpdateGamesMessage : INetworkMessage
{
    public string MessageName { get => "UpdateGames"; }
    public string[]? GameNames { get; set;}
}

public class GameEndedMessage : INetworkMessage
{
    public string MessageName { get => "GameEnded"; }
    public string? GameId { get; set;}
    public string? Reason { get; set;}
}