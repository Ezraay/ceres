using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Entities;
using System.ComponentModel;

public class GamesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? GameId {get; set;}
    private readonly GameManagerFactory _gameManagerFactory;
    public GameUser? Player1;
    public GameUser? Player2;

    public GamesModel(GameManagerFactory gameManagerFactory){
        _gameManagerFactory = gameManagerFactory;
    }

    public void OnGet()
    {
        try
        {
            if (Guid.TryParse(GameId, out var GameIdGuid)){
                Player1 = _gameManagerFactory.GetGameManagerById(GameIdGuid)?.Player1;
                Player2 = _gameManagerFactory.GetGameManagerById(GameIdGuid)?.Player2;
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