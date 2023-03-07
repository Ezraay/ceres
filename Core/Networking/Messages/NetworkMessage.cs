using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Newtonsoft.Json;

public interface INetworkMessage 
{
    [JsonIgnore]
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
    public string[] GameNames { get; set;} = Array.Empty<string>();
}

public class GameEndedMessage : INetworkMessage
{
    public string MessageName { get => "GameEnded"; }
    public string GameId { get; set;} = "";
    public string Reason { get; set;} = "";
}

public class PlayerSentCommandMessage : EventArgs, INetworkMessage
{
    public string MessageName { get => "PlayerSentCommand"; }
    public string GameId { get; set;} = "";
    public string UserId { get; set;} = "";
    public IClientCommand? Command { get; set;}
}

public class ClientsListMessage : INetworkMessage
{
    public string MessageName {get => "ClientsList"; }
    // public  IEnumerable<KeyValuePair<Guid, GameUser>> LobbyUsers { get; set;}
    public  GameUser[] LobbyUsers { get; set;}
}

public class ReceiveMessageMessage : INetworkMessage
{
    public string MessageName { get => "ReceiveMessage"; }
    public string UserName { get; set;} = "Unknown User";
    public string MessageText { get; set;} = "Empty Message";
}

public class GoToGameMessage : INetworkMessage
{
    public string MessageName { get => "GoToGame"; }
    public Guid GameId { get; set;} 
    public Guid UserId { get; set;} 
}

public class JoinedGame : INetworkMessage
{
    public string MessageName { get => "JoinedGame"; }
    public string GameJoiningResult;
}

public class UpdatePlayersNameMessage : INetworkMessage
{
    public string MessageName { get => "UpdatePlayersName"; }
    public string? Player1Name { get; set;} = "";
    public string? Player2Name { get; set;} = "";

}