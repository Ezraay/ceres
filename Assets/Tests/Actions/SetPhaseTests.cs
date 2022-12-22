using CardGame;
using NUnit.Framework;
using Tests.Actions.PlayerActions;

namespace Tests.Actions
{
    public class SetPhaseTests
    {
        [Test]
        public void ShouldSetPhase()
        {
            Battle battle = TestBattle.CreateTestBattle();
            BattlePhase phase = BattlePhase.End;
            SetPhase command = new SetPhase(phase);
            command.Execute(battle, null);
            Assert.AreEqual(phase, battle.BattlePhaseManager.Phase);
        }
    }
}