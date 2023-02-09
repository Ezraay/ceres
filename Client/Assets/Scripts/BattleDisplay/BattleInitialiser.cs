using Ceres.Client.BattleSystem;
using UnityEngine;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        private void Awake()
        {
            if (!BattleSystemManager.Started)
                BattleSystemManager.StartSinglePlayer();
        }
    }
}