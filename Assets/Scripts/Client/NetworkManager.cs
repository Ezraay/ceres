using System;
using UnityEngine;

namespace Ceres.Client
{
    [CreateAssetMenu(menuName = "Create NetworkManager", fileName = "NetworkManager", order = 0)]
    public class NetworkManager : ScriptableObject
    {
        public event Action OnConnected; // TODO: Call this when connected
        public event Action OnJoinQueue;
        
        public void Connect()
        {
            // TODO: Connect
            // var client = new NetworkClient() i dunno smth like this
        }

        public void JoinQueue()
        {
            // TODO: Join queue
        }
    }
}