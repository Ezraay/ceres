using System;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay.Networking
{
    public class SingleplayerProcessor : ICommandProcessor
    {
        public event Action<IServerAction> OnServerAction;
        public event Action<ClientBattle> OnStartBattle;
        private ServerBattle serverBattle;
        public ClientBattle ClientBattle { get; }
        
        public SingleplayerProcessor(ServerBattleStartConfig conditions)
        {
            IPlayer player1 = new StandardPlayer(new MultiCardSlot(), new MultiCardSlot());
            IPlayer player2 = new StandardPlayer(new MultiCardSlot(), new MultiCardSlot());
            player1.LoadDeck(conditions.Player1Deck);
            player2.LoadDeck(conditions.Player2Deck);
            serverBattle = new ServerBattle(player1, player2, conditions.Player1Turn);

            IPlayer myPlayer = new StandardPlayer(new MultiCardSlot(), new HiddenMultiCardSlot());
            IPlayer opponentPlayer = new StandardPlayer(new HiddenMultiCardSlot(), new HiddenMultiCardSlot());
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