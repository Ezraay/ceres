using System.Collections;
using System.Collections.Generic;
using Ceres.Client.BattleSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Ceres.Client
{
    public class QueueManager : MonoBehaviour
    {
        // [SerializeField] private NetworkManager networkManager;
        [SerializeField] [Scene] private string gameScene;

        void Start()
        {
            NetworkManager.OnJoinGame += OnJoinGame;
            NetworkManager.JoinQueue();
        }

        private void OnJoinGame(bool myTurn)
        {
            Debug.Log("Queue manager - OnJoinGame called");
            BattleSystemManager.StartBattle(myTurn);
            SceneManager.LoadScene(gameScene);
        }
    }
}
