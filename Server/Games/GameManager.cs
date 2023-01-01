public class GameManager
{

    public Guid GameId {get;}

    public GameManager(){
        GameId = Guid.NewGuid();
    }



}