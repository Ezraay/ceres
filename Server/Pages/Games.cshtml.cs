using Ceres.Core.BattleSystem;
using Ceres.Server.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ceres.Server.Pages;

public class GamesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? GameId {get; set;}
    private readonly IServerBattleManager serverBattleManager;
    public ServerBattle? ServerBattle;

    public GamesModel(IServerBattleManager serverBattleManager){
        this.serverBattleManager = serverBattleManager;
    }

    public void OnGet()
    {
        if (Guid.TryParse(GameId, out var gameIdGuid)){
            ServerBattle = serverBattleManager.FindServerBattleById(gameIdGuid);

        }

    }
    
}