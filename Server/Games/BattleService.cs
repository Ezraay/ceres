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
        var battle = battleManager.AllocateServerBattle();
 
        BattleTeam team1 = battle.TeamManager.CreateTeam();
        BattleTeam team2 = battle.TeamManager.CreateTeam();
        
        team1.AddPlayer(player1);
        user1.GameId = battle.Id;
        user1.ServerPlayer = player1;
        player1.LoadDeck(cardDeckLoader.Deck);

        team2.AddPlayer(player2);
        user2.GameId = battle.Id;
        user2.ServerPlayer = player2;
        player2.LoadDeck(cardDeckLoader.Deck);

        battle.OnPlayerAction += OnPlayerAction;
        battle.OnBattleAction += action => OnBattleAction(action, battle);
        battle.StartGame(playerOrder);

        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player1), battle.PhaseManager.Copy()), user1);
        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player2), battle.PhaseManager.Copy()), user2);

        SendListOfGamesUpdated();
    }

    private void OnBattleAction(IBattleAction action, ServerBattle battle)
    {
        switch (action)
        {
            case EndBattleAction endGameBattleAction:
                // Send to client, game ended
                List<GameUser> winners = new List<GameUser>();
                List<GameUser> losers = new List<GameUser>();

                foreach (BattleTeam team in endGameBattleAction.WinningTeams)
                foreach (IPlayer player in team.GetAllPlayers())
                {
                    GameUser? user = FindGameUser(player);
                    if (user != null)
                    {
                        winners.Add(user);
                        user.LeaveGame();
                    }
                }

                foreach (BattleTeam team in endGameBattleAction.LosingTeams)
                foreach (IPlayer player in team.GetAllPlayers())
                {
                    GameUser? user = FindGameUser(player);
                    if (user != null)
                    {
                        losers.Add(user);
                        user.LeaveGame();
                    }
                }
                
                networkService.SendServerBattleWon(winners.ToArray());
                networkService.SendServerBattleLost(losers.ToArray());
                
                battleManager.DeallocateServerBattle(battle);

                break;
            default:
                throw new SwitchExpressionException("Couldn't handle SystemAction of type " + action);
        }
    }

    private void OnPlayerAction(IPlayer player, IServerAction action)
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
        
        battle.TeamManager.RemovePlayer(user.ServerPlayer);
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


    private void PlayerCommandHandler(Guid gameId, GameUser user, IClientCommand command)
    {
        var serverBattle = battleManager.FindServerBattleById(gameId);
        if (serverBattle?.Id == user.GameId && user.ServerPlayer != null)
        {
            serverBattle.Execute(command, user.ServerPlayer);
        }
    }
}