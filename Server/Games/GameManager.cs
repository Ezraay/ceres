public class GameManager
{

    public Guid GameId {get;}

    public GameManager(){
        GameId = Guid.NewGuid();
    }

    public void EndGame(EndGameManagerReasons reason){
        // Console.WriteLine("");
    }  

}