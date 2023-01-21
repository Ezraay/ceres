using Ceres.Client.BattleSystem;
using UnityEngine;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        private void Awake()
        {
            BattleManager.StartBattle(true); // TODO: Get this from server
        }
    }
}