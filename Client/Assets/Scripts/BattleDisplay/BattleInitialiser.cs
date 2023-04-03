using System;
using Ceres.Client.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class BattleInitialiser : MonoBehaviour
    {
        // private BattleManager battleManager;
        // private NetworkManager networkManager;
        //
        // [Inject]
        // public void Construct(BattleManager battle, NetworkManager network)
        // {
        //     battleManager = battle;
        //     networkManager = network;
        // }
        //
        // private void Start()
        // {
        //     if (networkManager.IsConnected)
        //         battleManager.StartMultiplayer(networkManager);
        //     else
        //         battleManager.StartSinglePlayer();
        // }
    }
}