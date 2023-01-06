using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class GamesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? GameId {get; set;}
    
    public void OnGet()
    {
    }
    
    // public IActionResult OnPost([FromForm]string name)
    // {
    //     TempData["Name"] = name;
    //     return RedirectToPage("Index");
    // }
}