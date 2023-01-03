using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;

namespace Tests.Actions.PlayerActions
{
    public class TestBattle
    {
        public static Battle CreateTestBattle()
        {
            return CreateTestBattle(new Player());
        }
        
        public static Battle CreateTestBattle(IPlayer player1)
        {
            return CreateTestBattle(player1, new Player());
        }

        public static Battle CreateTestBattle(IPlayer player1, IPlayer player2)
        {
            return new Battle(player1, player2);
        }
    }
}