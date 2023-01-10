using Domain.Entities;

public class GameManager
{

    public Guid GameId {get;}
    public GameUser? Player1 {get; set;}
    public GameUser? Player2 {get; set;}

    public GameManager(){
        GameId = Guid.NewGuid();
    }

    public void EndGame(EndGameManagerReasons reason){
        // Console.WriteLine("");
    }  

}