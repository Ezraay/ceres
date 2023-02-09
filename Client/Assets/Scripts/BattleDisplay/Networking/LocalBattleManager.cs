using System;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay.Networking
{
    public class LocalBattleManager : IBattleManager
    {
        public event Action<IServerAction> OnServerAction;
        private ClientBattle clientBattle;
        private ServerBattle serverBattle;
        
        
        public LocalBattleManager(ServerBattleStartConfig conditions)
        {
            ServerPlayer player1 = new ServerPlayer();
            ServerPlayer player2 = new ServerPlayer();
            player1.LoadDeck(conditions.Player1Deck);
            player2.LoadDeck(conditions.Player2Deck);
            serverBattle = new ServerBattle(player1, player2, conditions.Player1Turn);

            AllyPlayer myPlayer = new AllyPlayer(new Card(player1.Champion.Card.Data), player1.Pile.Count);
            OpponentPlayer opponentPlayer = new OpponentPlayer(player2.Pile.Count);
            clientBattle = new ClientBattle(myPlayer, opponentPlayer, serverBattle.Player1Turn);

            serverBattle.OnPlayerAction += (player, action) =>
            {
                if (player == player1) // Accept actions sent to us 
                {
                    clientBattle.Apply(action);
                    OnServerAction?.Invoke(action);
                }
            };
        }

        public void ProcessCommand(IClientCommand command)
        {
            serverBattle.Execute(command, serverBattle.Player1);
        }
    }
}