using System;
using CardGame.Networking;
using Ceres.Client;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay.Networking
{
    public class NetworkedProcessor : ICommandProcessor
    {
        public event Action<IServerAction> OnServerAction;
        public event Action<BattleStartConditions> OnStartBattle;
        private ClientBattle ClientBattle { get; set; }
        private readonly NetworkManager networkManager;
        
        public NetworkedProcessor(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            
        }

        public IPlayer MyPlayer { get; private set; }

        public void Start()
        {
            networkManager.OnStartGame += battleStartConditions =>
            {
                ClientBattle = battleStartConditions.ClientBattle;
                if (battleStartConditions.PlayerId != null)
                    MyPlayer = ClientBattle.TeamManager.GetPlayer(battleStartConditions.PlayerId);
                OnStartBattle?.Invoke(battleStartConditions);
            };

            networkManager.OnBattleAction += (action) =>
            {
                ClientBattle.Execute(action);
                OnServerAction?.Invoke(action);
            };
        }

        public void ProcessCommand(IClientCommand command)
        {
            networkManager.SendCommand(command);
        }
    }
}