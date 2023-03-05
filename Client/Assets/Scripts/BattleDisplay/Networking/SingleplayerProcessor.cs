using System;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay.Networking
{
    public class SingleplayerProcessor : ICommandProcessor
    {
        public event Action<IServerAction> OnServerAction;
        private ServerBattle serverBattle;
        public ClientBattle ClientBattle { get; }
        
        public SingleplayerProcessor(ServerBattleStartConfig conditions)
        {
            ServerPlayer player1 = new ServerPlayer();
            ServerPlayer player2 = new ServerPlayer();
            player1.LoadDeck(conditions.Player1Deck);
            player2.LoadDeck(conditions.Player2Deck);
            serverBattle = new ServerBattle(player1, player2, conditions.Player1Turn);

            AllyPlayer myPlayer = new AllyPlayer(new Card(player1.Champion.Card.Data), player1.Pile.Count);
            OpponentPlayer opponentPlayer = new OpponentPlayer(player2.Pile.Count);
            ClientBattle = new ClientBattle(myPlayer, opponentPlayer, serverBattle.Player1Turn);

            serverBattle.OnPlayerAction += (player, action) =>
            {
                if (player == player1) // Accept actions sent to us 
                {
                    ClientBattle.Apply(action);
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