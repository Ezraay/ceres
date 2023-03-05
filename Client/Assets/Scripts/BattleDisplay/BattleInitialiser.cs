using Ceres.Client.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        [Inject]
        public void Construct(BattleManager battleManager, NetworkManager networkManager)
        {
            if (networkManager.IsConnected)
                battleManager.StartMultiplayer(networkManager);
            else
                battleManager.StartSinglePlayer();
        }
    }
}