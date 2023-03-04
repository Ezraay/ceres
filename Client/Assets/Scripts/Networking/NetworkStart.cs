using System;
using Ceres.Client.Networking;
using UnityEngine;
using Zenject;

namespace CardGame.Networking
{
    public class NetworkStart : MonoBehaviour
    {
        private SignalRManager signalRManager;

        [Inject]
        public void Construct(SignalRManager signalRManager)
        {
            this.signalRManager = signalRManager;
        }
        
        private void Start()
        {
            signalRManager.Connect();
        }
    }
}