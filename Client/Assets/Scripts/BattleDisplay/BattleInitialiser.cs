using Ceres.Client.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        [Inject]
        public void Construct(BattleSystemManager battleSystemManager)
        {
            if (!battleSystemManager.IsStarted)
                battleSystemManager.StartSinglePlayer();
        }
    }
}