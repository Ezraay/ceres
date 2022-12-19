using CardGame;
using NUnit.Framework;

namespace Tests.Actions.PlayerActions
{
    public class AdvancePhaseTests
    {
        [Test]
        public void CanExecuteTrue()
        {
            Battle battle = TestBattle.CreateTestBattle();

            bool canExecute1 = new AdvancePhase().CanExecute(battle, battle.Player1);
            bool canExecute2 = new AdvancePhase().CanExecute(battle, battle.Player2);

            Assert.AreEqual(canExecute1, battle.Player1Priority);
            Assert.AreEqual(canExecute2, !battle.Player1Priority);
        }

        [Test]
        public void Execute()
        {
            Battle battle = TestBattle.CreateTestBattle();

            BattlePhase phase = battle.BattlePhaseManager.Phase;
            battle.Execute(new AdvancePhase());
            battle.Tick();
            Assert.IsTrue(phase + 1 == battle.BattlePhaseManager.Phase);
        }
    }
}