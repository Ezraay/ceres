using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;

namespace Ceres.Server.Services;

public class BattleService : IBattleService
{
    private readonly IServerBattleManager battleManager;
    private readonly ISignalRService networkService;
    private readonly CardDeckLoader cardDeckLoader;


    public BattleService(ISignalRService networkService, IServerBattleManager battleManager, CardDeckLoader cardDeckLoader) 
    {
        this.battleManager = battleManager;
        this.networkService = networkService;
        this.cardDeckLoader = cardDeckLoader;

        this.networkService.OnTryToJoinGame += JoinBattle;
        this.networkService.OnUsersReadyToPlay += StartBattle;
        this.networkService.OnPlayerSentCommand += PlayerCommandHandler;
        this.networkService.OnUserConnectedToLobby += UserConnectedToLobby;
    }



    public void PlayerLeftGame(string connectionId)
    {
        throw new NotImplementedException();
    }

    private void SendListOfGamesUpdated()
    {
        var games = battleManager.ServerBattles().Values.Select(v=> v.GameId.ToString()).ToArray();
        networkService.SendListOfGamesUpdated(games);
    }

    public void SendServerBattleEnded(Guid gameId, string reason)
    {
        networkService.SendServerBattleEnded(gameId, reason);
    }


    private void StartBattle(GameUser user1, GameUser user2)
    {
        var battle = battleManager.AllocateServerBattle();
                        
        user2.GameId = battle.GameId;
        battle.Player2 = new StandardPlayer(new MultiCardSlot(), new MultiCardSlot());
        user2.ServerPlayer = battle.Player2;

        user1.GameId = battle.GameId;
        battle.Player1 = new StandardPlayer(new MultiCardSlot(), new MultiCardSlot());
        user1.ServerPlayer = battle.Player1;

        battle.OnPlayerAction += OnPlayerAction;

        networkService.SendUserGoToGame(user1);
        networkService.SendUserGoToGame(user2);

        SendListOfGamesUpdated();
    }

    private void OnPlayerAction(IPlayer player, IServerAction action)
    {
        var gameUser = FindGameUser(player);
        if (gameUser != null)
        {
            networkService.SendPlayerAction(gameUser, action);
        }
        
    }

    private void UserConnectedToLobby(GameUser user)
    {
        SendListOfGamesUpdated();
    }
    
    public void StopBattle(Guid battleId, string reason)
    {
        
    }

    private void UpdatePlayersName(ServerBattle serverBattle){
        networkService.UpdatePlayersName(serverBattle.GameId.ToString(), FindGameUser(serverBattle.Player1)?.UserName,
            FindGameUser(serverBattle.Player2)?.UserName);  
    }

    private GameUser? FindGameUser(IPlayer serverPlayer)
    {
        return networkService.GetUserByServerPlayer(serverPlayer);
    }
    

    private void JoinBattle(Guid battleId, GameUser gameUser)
    {
        var serverBattle = battleManager.FindServerBattleById(battleId);
        if (serverBattle == null)
            return;
        if (gameUser.ServerPlayer == null)
        {
            networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsSpectator);
            UpdatePlayersName(serverBattle);  
        }
        else
        {
            if (gameUser.ServerPlayer.Equals(serverBattle.Player1))
            {
                serverBattle.Player1.LoadDeck(cardDeckLoader.Deck);
                networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsPlayer1);
                UpdatePlayersName(serverBattle);
                return;
            }

            if (gameUser.ServerPlayer.Equals(serverBattle.Player2))
            {
                serverBattle.Player2.LoadDeck(cardDeckLoader.Deck);
                networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsPlayer2);
                UpdatePlayersName(serverBattle);
                return;
            }
        }

    }


    private void PlayerCommandHandler(Guid gameId, GameUser user, IClientCommand command)
    {
        var serverBattle = battleManager.FindServerBattleById(gameId);
        if (serverBattle?.GameId == user.GameId && user.ServerPlayer != null)
        {
            serverBattle?.Execute(command, user.ServerPlayer);
        }
    }


}