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
        private NetworkManager networkManager;

        [Inject]
        public void Construct(NetworkManager manager)
        {
            networkManager = manager;
        }
        
        void Start()
        {
            networkManager.JoinQueue();
        }
    }
}
