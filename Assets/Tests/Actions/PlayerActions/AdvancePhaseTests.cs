using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using NUnit.Framework;

namespace Tests.Actions.PlayerActions
{
    public class AdvancePhaseTests
    {
        [Test]
        public void CantExecuteWithoutPriority()
        {
            Battle battle = TestBattle.CreateTestBattle();
            AdvancePhase command = new AdvancePhase();
            Assert.IsFalse(command.CanExecute(battle, battle.Player2));
        }

        [Test]
        public void CanExecuteWithPriority()
        {
            Battle battle = TestBattle.CreateTestBattle();
            AdvancePhase command = new AdvancePhase();
            Assert.IsTrue(command.CanExecute(battle, battle.Player1));
        }

        [Test]
        public void Execute()
        {
            Battle battle = TestBattle.CreateTestBattle();
            BattlePhase phase = battle.BattlePhaseManager.Phase;
            battle.ExecuteImmediately(new AdvancePhase());
            Assert.IsTrue(phase + 1 == battle.BattlePhaseManager.Phase);
        }
    }
}