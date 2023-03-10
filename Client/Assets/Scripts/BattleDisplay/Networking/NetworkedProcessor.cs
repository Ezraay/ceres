using System;
using Ceres.Client;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay.Networking
{
    public class NetworkedProcessor : ICommandProcessor
    {
        public event Action<IServerAction> OnServerAction;
        public event Action<ClientBattle> OnStartBattle;
        public ClientBattle ClientBattle { get; private set; }
        private NetworkManager networkManager;
        
        public NetworkedProcessor(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            
            networkManager.OnStartGame += config =>
            {
                IPlayer myPlayer = new StandardPlayer(new MultiCardSlot(), new HiddenMultiCardSlot());
                IPlayer opponentPlayer = new StandardPlayer(new HiddenMultiCardSlot(), new HiddenMultiCardSlot());
                ClientBattle = new ClientBattle(myPlayer, opponentPlayer, config.MyTurn);
                OnStartBattle?.Invoke(ClientBattle);
            };

            networkManager.OnBattleAction += action =>
            {
                ClientBattle.Apply(action);
                OnServerAction?.Invoke(action);
            };
        }
        
        public void ProcessCommand(IClientCommand command)
        {
            networkManager.SendCommand(command);
        }
    }
}