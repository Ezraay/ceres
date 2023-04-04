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

        this.networkService.OnTryToJoinGame += JoinBattle;
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
        var battle = battleManager.AllocateServerBattle();

        IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
        IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
        BattleTeam team1 = battle.TeamManager.CreateTeam();
        BattleTeam team2 = battle.TeamManager.CreateTeam();
        battle.TeamManager.AddPlayer(player1, team1);
        battle.TeamManager.AddPlayer(player2, team2);

        user1.GameId = battle.Id;
        user1.ServerPlayer = player1;
        player1.LoadDeck(cardDeckLoader.Deck);

        user2.GameId = battle.Id;
        user2.ServerPlayer = player2;
        player2.LoadDeck(cardDeckLoader.Deck);

        battle.OnPlayerAction += OnPlayerAction;
        battle.OnBattleAction += action => OnBattleAction(action, battle);
        battle.StartGame();

        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player1)), user1);
        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player2)), user2);

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


    private void JoinBattle(Guid battleId, GameUser gameUser)
    {
        var serverBattle = battleManager.FindServerBattleById(battleId);
        if (serverBattle == null)
            return;
        // if (gameUser.ServerPlayer == null)
        // {
        //     networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsSpectator);
        //     UpdatePlayersName(serverBattle);
        // }
        // else
        // {
        //     if (gameUser.ServerPlayer.Equals(serverBattle.Player1))
        //     {
        //         serverBattle.Player1.LoadDeck(cardDeckLoader.Deck);
        //         networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsPlayer1);
        //         UpdatePlayersName(serverBattle);
        //         return;
        //     }
        //
        //     if (gameUser.ServerPlayer.Equals(serverBattle.Player2))
        //     {
        //         serverBattle.Player2.LoadDeck(cardDeckLoader.Deck);
        //         networkService.UserJoinedGame(gameUser, battleId, JoinGameResults.JoinedAsPlayer2);
        //         UpdatePlayersName(serverBattle);
        //     }
        // }
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