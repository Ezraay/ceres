using CardGame;

namespace Tests.Actions.PlayerActions
{
    public class TestBattle
    {
        public static Battle CreateTestBattle()
        {
            Player player1 = new Player();
            Player player2 = new Player();
            Battle battle = new Battle(player1, player2);
            return battle;
        }
    }
}