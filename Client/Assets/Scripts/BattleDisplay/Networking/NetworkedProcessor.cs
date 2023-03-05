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
        public ClientBattle ClientBattle { get; private set; }
        private NetworkManager networkManager;
        
        public NetworkedProcessor(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            
            networkManager.OnStartGame += config =>
            {
                AllyPlayer myPlayer = new AllyPlayer(new Card(config.Champion), config.PileCount);
                OpponentPlayer opponentPlayer = new OpponentPlayer(config.OpponentPileCount);
                ClientBattle = new ClientBattle(myPlayer, opponentPlayer, config.MyTurn);
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