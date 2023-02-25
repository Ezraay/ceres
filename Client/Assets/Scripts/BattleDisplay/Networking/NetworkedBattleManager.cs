using System;
using Ceres.Client;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay.Networking
{
    public class NetworkedBattleManager : IBattleManager
    {
        public event Action<IServerAction> OnServerAction;
        private ClientBattle clientBattle;
        private NetworkManager networkManager;
        
        public NetworkedBattleManager(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            
            networkManager.OnStartGame += config =>
            {
                AllyPlayer myPlayer = new AllyPlayer(new Card(config.Champion), config.PileCount);
                OpponentPlayer opponentPlayer = new OpponentPlayer(config.OpponentPileCount);
                clientBattle = new ClientBattle(myPlayer, opponentPlayer, config.MyTurn);
            };

            networkManager.OnBattleAction += action =>
            {
                clientBattle.Apply(action);
                OnServerAction?.Invoke(action);
            };
        }
        
        public void ProcessCommand(IClientCommand command)
        {
            networkManager.SendCommand(command);
        }
    }
}