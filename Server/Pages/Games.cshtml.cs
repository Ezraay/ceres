using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ceres.Core.Entities;
using System.ComponentModel;
using Ceres.Core.BattleSystem;

public class GamesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? GameId {get; set;}
    private readonly IServerBattleManager _serverBattleManager;
    public ServerBattle? serverBattle;
    // public GameUser? Player1;
    // public GameUser? Player2;

    public GamesModel(IServerBattleManager serverBattleManager){
        _serverBattleManager = serverBattleManager;
    }

    public void OnGet()
    {
        try
        {
            if (Guid.TryParse(GameId, out var GameIdGuid)){
                serverBattle = _serverBattleManager.FindServerBattleById(GameIdGuid);

                // Player1 =  serverBattle?.Player1;
                // Player2 = serverBattle?.Player2;
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }

    }
    
    // public IActionResult OnPost([FromForm]string name)
    // {
    //     TempData["Name"] = name;
    //     return RedirectToPage("Index");
    // }
}