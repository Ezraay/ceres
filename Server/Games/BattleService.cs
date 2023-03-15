using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Server.Services;

namespace Ceres.Server.Games;

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
        this.networkService.OnPlayerLeftGame += PlayerLeftGame;
    }




    private void SendListOfGamesUpdated()
    {
        var games = battleManager.ServerBattles().Values.Select(v=> v.GameId.ToString()).ToArray();
        networkService.SendListOfGamesUpdated(games);
    }




    private void StartBattle(GameUser user1, GameUser user2)
    {
        var battle = battleManager.AllocateServerBattle();
                        
        user2.GameId = battle.GameId;
        battle.Player2 = new ServerPlayer();
        user2.ServerPlayer = battle.Player2;

        user1.GameId = battle.GameId;
        battle.Player1 = new ServerPlayer();
        user1.ServerPlayer = battle.Player1;

        battle.OnPlayerAction += OnPlayerAction;

        networkService.SendUserGoToGame(user1);
        networkService.SendUserGoToGame(user2);

        SendListOfGamesUpdated();
    }

    private void OnPlayerAction(ServerPlayer player, IServerAction action)
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
    
    private void PlayerLeftGame(GameUser user)
    {
        var battle = battleManager.FindServerBattleById(user.GameId);
        if (battle == null) { return; }

        if (battle.Player1 == user.ServerPlayer)
        {
            battleManager.EndServerBattle(battle.GameId, EndServerBattleReasons.Player1Left);
            networkService.SendServerBattleEnded(battle.GameId,EndServerBattleReasons.Player1Left);
        }
        else
        {
            if (battle.Player2 == user.ServerPlayer)
            {
                battleManager.EndServerBattle(battle.GameId, EndServerBattleReasons.Player2Left);
                networkService.SendServerBattleEnded(battle.GameId,EndServerBattleReasons.Player2Left);
                return;
            } 
            
            battleManager.EndServerBattle(battle.GameId, EndServerBattleReasons.ReasonUnknown);
        }
        
        SendListOfGamesUpdated();
        
    }

    private void UpdatePlayersName(ServerBattle serverBattle){
        networkService.UpdatePlayersName(serverBattle.GameId.ToString(), FindGameUser(serverBattle.Player1)?.UserName,
            FindGameUser(serverBattle.Player2)?.UserName);  
    }

    private GameUser? FindGameUser(ServerPlayer serverPlayer)
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
            }
        }

    }


    private void PlayerCommandHandler(Guid gameId, GameUser user, IClientCommand command)
    {
        var serverBattle = battleManager.FindServerBattleById(gameId);
        if (serverBattle?.GameId == user.GameId && user.ServerPlayer != null)
        {
            serverBattle.Execute(command, user.ServerPlayer);
        }
    }


}