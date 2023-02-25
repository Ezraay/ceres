using System.Collections.Concurrent;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Ceres.Server.Services;
public class NetworkService
{
    private readonly IHubContext<GameHub> gameHub;
    private readonly IHubContext<LobbyHub> lobbyHub;

    public NetworkService( IHubContext<GameHub> gameHub, IHubContext<LobbyHub> lobbyHub)
    {
        this.gameHub = gameHub;
        this.lobbyHub = lobbyHub;
    }

    private void  SendHubMessage(IHubContext<Hub> context, string connectionId, INetworkMessage message)
    {
        context.Clients.Client(connectionId).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }
    public void SendHubAllMessage(IHubContext<Hub> context, INetworkMessage message)
    {
        context.Clients.All.SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }
    private void SendHubGroupMessage(IHubContext<Hub> context, string groupId, INetworkMessage message)
    {
        context.Clients.Group(groupId).SendAsync(message.MessageName, message).GetAwaiter().GetResult();
    }

    public void SendPlayerAction(ServerPlayer player, IServerAction action){
        var gameId = ((GameUser)player).GameId;
        var connectionId = ((GameUser)player).ConnectionId;
        Console.WriteLine("Sending action: " + action + " to: " + connectionId);
        var msg = new ServerActionMessage() {Action = action};
        SendHubMessage(gameHub, connectionId, msg);
    }

    public void SendServerBattleEnded(Guid gameId, string reason){
        var msg = new GameEndedMessage() {GameId = gameId.ToString(), Reason = reason};
        SendHubGroupMessage(gameHub, msg.GameId, msg);
    }

    public void SendListOfGamesUpdated(ConcurrentDictionary<Guid, ServerBattle> battles){
        var msg = new UpdateGamesMessage() {GameNames = battles.Keys.Select(key => key.ToString()).ToArray()};
        SendHubAllMessage(lobbyHub, msg);
    }
}