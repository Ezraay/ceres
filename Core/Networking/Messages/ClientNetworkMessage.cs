using System;
using Ceres.Core.BattleSystem;

namespace Ceres.Core.Networking.Messages
{
    public class ClientReadyToPlayNetworkMessage:INetworkMessage
    {
        public string MessageName => "UserIsReadyToPlay";
        public string UserName = "";
        public bool Ready;
    }
    
    public class ClientSendMessageNetworkMessage:INetworkMessage
    {
        public string MessageName => "SendMessage";
        public string UserName = "";
        public string Message = "";
    }
    
    public class ClientChangeUserNameNetworkMessage:INetworkMessage
    {
        public string MessageName => "ChangeUserName";
        public string NewName = "";
    }    
    public class ClientPlayerSentCommandMessage:INetworkMessage
    {
        public string MessageName => "PlayerSentCommand";
        public IClientCommand? PlayerCommand;
        public Guid GameId;
        public Guid UserId;
    }
    public class ClientJoinGameMessage:INetworkMessage
    {
        public string MessageName => "JoinGame";
        public Guid GameId;
        public Guid UserId;
    }

}