using Ceres.Client.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        [Inject]
        public void Construct(BattleSystemManager battleSystemManager, NetworkManager networkManager)
        {
            if (networkManager.Connected)
                battleSystemManager.StartMultiplayer(networkManager);
            else
                battleSystemManager.StartSinglePlayer();
        }
    }
}