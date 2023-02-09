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

        public NetworkedBattleManager()
        {
            NetworkManager.OnJoinGame += myTurn =>
            {
                string cardDataPath = "Data/Testing Deck";
                TextAsset text = Resources.Load<TextAsset>(cardDataPath);
                IDeck baseDeck = new CSVDeck(BattleSystemManager.CardDatabase, text.text.Trim());

                // TODO: Load this from server: ClientBattleStartConfig
                AllyPlayer myPlayer = new AllyPlayer(new Card(baseDeck.GetChampion()), baseDeck.GetPile().Length);
                OpponentPlayer opponentPlayer = new OpponentPlayer(baseDeck.GetPile().Length);
                clientBattle = new ClientBattle(myPlayer, opponentPlayer, myTurn);
            };

            NetworkManager.OnBattleAction += action =>
            {
                clientBattle.Apply(action);
                OnServerAction?.Invoke(action);
            };
        }
        
        public void ProcessCommand(IClientCommand command)
        {
            NetworkManager.SendCommand(command);
        }
    }
}