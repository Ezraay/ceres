using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ceres.Client
{
    public class QueueManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField, Scene] private string gameScene;

        void Start()
        {
            networkManager.OnJoinQueue += OnJoinQueue;
            networkManager.JoinQueue();
        }

        private void OnJoinQueue()
        {
            SceneManager.LoadScene(gameScene);
        }
    }
}
