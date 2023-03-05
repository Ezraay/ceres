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
    }
}