using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Enums;

namespace Ceres.Server.Services;

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
    }


    public void PlayerLeftGame(string connectionId)
    {
        throw new NotImplementedException();
    }

    private void SendListOfGamesUpdated()
    {
        var games = battleManager.ServerBattles().Keys.ToArray();
        networkService.SendListOfGamesUpdated(games);
    }

    public void SendServerBattleEnded(Guid gameId, string reason)
    {
        networkService.SendServerBattleEnded(gameId, reason);
    }


    private void StartBattle(GameUser user1, GameUser user2)
    {
        Guid battleId = Guid.NewGuid();
        var battle = battleManager.AllocateServerBattle(battleId);

        IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
        IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
        BattleTeam team1 = new BattleTeam(Guid.NewGuid(), player1);
        BattleTeam team2 = new BattleTeam(Guid.NewGuid(), player2);
        
        battle.TeamManager.AddTeam(team1);
        battle.TeamManager.AddTeam(team2);
        battle.TeamManager.MakeEnemies(team1, team2);

        user1.GameId = battleId;
        user1.ServerPlayer = player1;
        player1.LoadDeck(cardDeckLoader.Deck);

        user2.GameId = battleId;
        user2.ServerPlayer = player2;
        player2.LoadDeck(cardDeckLoader.Deck);

        battle.OnPlayerAction += OnPlayerAction;

        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player1)), user1);
        networkService.SendUserGoToGame(new ClientBattle(battle.TeamManager.SafeCopy(player2)), user2);

        SendListOfGamesUpdated();
    }

    private void OnPlayerAction(IPlayer player, IServerAction action)
    {
        var gameUser = FindGameUser(player);
        if (gameUser != null) networkService.SendPlayerAction(gameUser, action);
    }

    private void UserConnectedToLobby(GameUser user)
    {
        SendListOfGamesUpdated();
    }

    public void StopBattle(Guid battleId, string reason) { }

    private void UpdatePlayersName(ServerBattle serverBattle)
    {
        // networkService.UpdatePlayersName(serverBattle.GameId.ToString(), FindGameUser(serverBattle.Player1)?.UserName,
        // FindGameUser(serverBattle.Player2)?.UserName);  
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
    }


    private void PlayerCommandHandler(Guid gameId, GameUser user, IClientCommand command)
    {
        var serverBattle = battleManager.FindServerBattleById(gameId);
        if (gameId == user.GameId && user.ServerPlayer != null) serverBattle?.Execute(command, user.ServerPlayer);
    }
}