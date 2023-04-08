using System;
using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;

namespace Ceres.Core.Networking.Messages
{
    public class ServerActionMessage : INetworkMessage
    {
        public string MessageName => "ServerAction";
        public IServerAction Action { get; set;}
    }

    public class UpdateGamesMessage : INetworkMessage
    {
        public string MessageName => "UpdateGames";
        public Guid[] GameIds { get; set;}
    }

    public class GameEndedMessage : INetworkMessage
    {
        public string MessageName => "GameEnded";
        public EndBattleReason Reason { get; set;}
    }

    public class PlayerSentCommandMessage : EventArgs, INetworkMessage
    {
        public string MessageName => "PlayerSentCommand";
        public string GameId { get; set;} = "";
        public string UserId { get; set;} = "";
        public IClientCommand Command { get; set;}
    }

    public class ClientsListMessage : INetworkMessage
    {
        public string MessageName => "ClientsList";
        public  GameUser[] LobbyUsers { get; set;}
    }

    public class ReceiveMessageMessage : INetworkMessage
    {
        public string MessageName => "ReceiveMessage";
        public string UserName { get; set;} = "Unknown User";
        public string MessageText { get; set;} = "Empty Message";
    }

    public class GoToGameMessage : INetworkMessage
    {
        public string MessageName => "GoToGame";
        public Guid GameId { get; set;} 
        public Guid UserId { get; set;} 
        public ClientBattle ClientBattle { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class JoinedGame : INetworkMessage
    {
        public string MessageName => "JoinedGame";
        public string GameJoiningResult;
    }

    public class UpdatePlayersNameMessage : INetworkMessage
    {
        public string MessageName => "UpdatePlayersName";
        public IEnumerable<BattleTeam>? Allies ;
        public IEnumerable<BattleTeam>? Enemies ;
    }
}