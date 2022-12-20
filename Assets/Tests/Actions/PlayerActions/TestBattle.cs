using CardGame;

namespace Tests.Actions.PlayerActions
{
    public class TestBattle
    {
        public static Battle CreateTestBattle()
        {
            return CreateTestBattle(new Player());
        }
        
        public static Battle CreateTestBattle(Player player1)
        {
            return CreateTestBattle(player1, new Player());
        }

        public static Battle CreateTestBattle(Player player1, Player player2)
        {
            return new Battle(player1, player2);
        }
    }
}