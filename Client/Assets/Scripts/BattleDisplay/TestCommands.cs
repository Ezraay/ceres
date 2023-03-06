using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client.BattleSystem
{
    public class TestCommands : MonoBehaviour
    {
        private BattleManager battleManager;

        [Inject]
        public void Construct(BattleManager battleManager)
        {
            this.battleManager = battleManager;
        }

        public void TestDrawCommand()
        {
            battleManager.Execute(new TestDrawCommand());
        }

        public void TestAscendCommand()
        {
            Card card = battleManager.Battle.AllyPlayer.Hand.Cards[0];
            battleManager.Execute(new AscendCommand(card.ID));
        }

        public void TestOpponentDrawAndAscend()
        {
           battleManager.FakeAction(new OpponentDrawCardAction());
            battleManager.FakeAction(new OpponentSummonAction(MultiCardSlotType.Hand, 1, 0, Card.TestCard()));
        }
    }
}