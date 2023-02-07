using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ceres.Client.BattleSystem
{
    public class TestCommands : MonoBehaviour
    {
        public void TestDrawCommand()
        {
            BattleManager.Execute(new TestDrawCommand());
        }
    }
}