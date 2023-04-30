using System.Runtime.CompilerServices;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.Entities;
using Ceres.Server.Services;

namespace Ceres.Server.Games;

public class BattleService : IBattleService
{
    private readonly IServerBattleManager battleManager;
    private readonly CardDeckLoader cardDeckLoader;
    private readonly ISignalRService networkService;


    public BattleService(ISignalRService networkService, IServerBattleManager battleManager,
        CardDeckLoader cardDeckLoader)
    {
        this.battleManager = battleManager;
        this.networkService = networkService;
        this.cardDeckLoader = cardDeckLoader;

        this.networkService.OnTryToJoinBattle += TryToJoinBattle;
        this.networkService.OnUsersReadyToPlay += StartBattle;
        this.networkService.OnPlayerSentCommand += PlayerCommandHandler;
        this.networkService.OnUserConnectedToLobby += UserConnectedToLobby;
        this.networkService.OnPlayerLeftGame += PlayerLeftGame;
    }


    private void SendListOfGamesUpdated()
    {
        var games = battleManager.ServerBattles().Keys.ToArray();
        networkService.SendListOfGamesUpdated(games);
    }


    private void StartBattle(GameUser user1, GameUser user2)
    {
        IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
        IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());

        List<IPlayer> playerOrder = new List<IPlayer>() {player1, player2};
        var battle = battleManager.AllocateServerBattle(player1, player2);
 
        // BattleTeam team1 = battle.TeamManager.CreateTeam();
        // BattleTeam team2 = battle.TeamManager.CreateTeam();
        
        // team1.AddPlayer(player1);
        user1.GameId = battle.Id;
        user1.ServerPlayer = player1;
        player1.LoadDeck(cardDeckLoader.Deck);

        // team2.AddPlayer(player2);
        user2.GameId = battle.Id;
        user2.ServerPlayer = player2;
        player2.LoadDeck(cardDeckLoader.Deck);

        battle.OnPlayerAction += OnPlayerAction;
        battle.OnBattleAction += action => OnBattleAction(action, battle);
        battle.StartGame(playerOrder);

        networkService.SendUserGoToGame(new ClientBattle(battle.PhaseManager.Copy(), player1.GetAllyCopy(), player2.GetEnemyCopy()), user1);
        networkService.SendUserGoToGame(new ClientBattle(battle.PhaseManager.Copy(), player1.GetEnemyCopy(), player2.GetAllyCopy()), user2);

        SendListOfGamesUpdated();
    }

    private void OnBattleAction(IBattleAction action, ServerBattle battle)
    {
        switch (action)
        {
            case EndBattleAction endGameBattleAction:
                // Send to client, game ended

                GameUser? winner = FindGameUser(endGameBattleAction.Winner);
                if (winner != null)
                {
                    winner.LeaveGame();
                    networkService.SendServerBattleWon(winner);
                }
                
                GameUser? loser = FindGameUser(endGameBattleAction.Loser);
                if (loser != null)
                {
                    loser.LeaveGame();
                    networkService.SendServerBattleLost(loser);
                }
                
                battleManager.DeallocateServerBattle(battle);

                break;
            default:
                throw new SwitchExpressionException("Couldn't handle SystemAction of type " + action);
        }
    }

    private void OnPlayerAction(IPlayer player, ServerAction action)
    {
        var gameUser = FindGameUser(player);
        if (gameUser != null)
            networkService.SendPlayerAction(gameUser, action);
    }

    private void UserConnectedToLobby(GameUser user)
    {
        SendListOfGamesUpdated();
    }

    private void PlayerLeftGame(GameUser user)
    {
        var battle = battleManager.FindServerBattleById(user.GameId);
        if (battle == null)
            return;
        
        battle.RemovePlayer(user.ServerPlayer);
        user.GameId = Guid.Empty;

        SendListOfGamesUpdated();
    }

    private GameUser? FindGameUser(IPlayer serverPlayer)
    {
        return networkService.GetUserByServerPlayer(serverPlayer);
    }


    private void TryToJoinBattle(Guid battleId, GameUser gameUser)
    {
        var serverBattle = battleManager.FindServerBattleById(battleId);
        if (serverBattle == null)
            return;
        
        // var player = gameUser.ServerPlayer;
        // networkService.UpdatePlayersName(serverBattle.Id.ToString(), serverBattle.TeamManager.GetAllies(player),
        //     serverBattle.TeamManager.GetEnemies(player));
    }


    private void PlayerCommandHandler(Guid gameId, GameUser user, ClientCommand command)
    {
        var serverBattle = battleManager.FindServerBattleById(gameId);
        if (serverBattle?.Id == user.GameId && user.ServerPlayer != null)
        {
            serverBattle.AddToStack(command, user.ServerPlayer);
        }
    }
}