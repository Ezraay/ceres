using System.Collections;
using System.Collections.Generic;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class QueueManager : MonoBehaviour
    {
        // [SerializeField] private NetworkManager networkManager;
        [SerializeField] [Scene] private string gameScene;
        private NetworkManager networkManager;

        [Inject]
        public void Construct(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }
        
        void Start()
        {
            networkManager.OnStartGame += OnStartGame;
            networkManager.JoinQueue();
        }

        private void OnStartGame(ClientBattleStartConfig _)
        {
            SceneManager.LoadScene(gameScene);
        }
    }
}
