using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ceres.Core.Entities;
using System.ComponentModel;
using Ceres.Core.BattleSystem;

public class GamesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? GameId {get; set;}
    private readonly ServerBattleFactory _serverBattleFactory;
    public ServerBattle? serverBattle;
    public GameUser? Player1;
    public GameUser? Player2;

    public GamesModel(ServerBattleFactory gameManagerFactory){
        _serverBattleFactory = gameManagerFactory;
    }

    public void OnGet()
    {
        try
        {
            if (Guid.TryParse(GameId, out var GameIdGuid)){
                serverBattle = _serverBattleFactory.GetServerBattleById(GameIdGuid);

                Player1 = (GameUser?)serverBattle?.Player1;
                Player2 = (GameUser?)serverBattle?.Player2;
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