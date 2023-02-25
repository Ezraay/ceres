using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client.BattleSystem
{
    public class TestCommands : MonoBehaviour
    {
        private BattleSystemManager battleSystemManager;

        [Inject]
        public void Construct(BattleSystemManager battleSystemManager)
        {
            this.battleSystemManager = battleSystemManager;
        }

        public void TestDrawCommand()
        {
            battleSystemManager.Execute(new TestDrawCommand());
        }
    }
}