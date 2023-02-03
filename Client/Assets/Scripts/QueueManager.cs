using System.Collections;
using System.Collections.Generic;
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

        private void OnJoinGame()
        {
            Debug.Log("Queue manager - OnJoinGame called");
            SceneManager.LoadScene(gameScene);
        }
    }
}
