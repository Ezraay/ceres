using System;
using Ceres.Core.Enums;

namespace Ceres.Core.BattleSystem
{
    public class ServerBattle
    {

        public Guid GameId {get;}
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        public ServerPlayer Player1;
        public bool Player1Turn { get; private set; }

        public ServerPlayer Player2;
        
        public event Action<ServerPlayer, IServerAction> OnPlayerAction;

        public ServerBattle(ServerPlayer player1, ServerPlayer player2, bool player1Turn)
        {
            GameId = Guid.NewGuid();
            Player1 = player1;
            Player2 = player2;
            Player1Turn = player1Turn;
            PhaseManager.OnTurnEnd += () => Player1Turn = !Player1Turn;
        }

        public bool IsPriorityPlayer(ServerPlayer player)
        {
            switch (PhaseManager.Phase)
            {
                default:
                    return player == Player1 ? Player1Turn : !Player1Turn;
            }
        }

        // TODO: Run this from the client
        public void Execute(IClientCommand command, ServerPlayer author)
        {
            if (command.CanExecute(this, author))
            {
                // Client could be cheating or out of sync
                // TODO: Send a failure response 
            }

            command.Apply(this, author);

            foreach (var serverAction in command.GetActionsForAlly())
                if (author == Player1)
                    OnPlayerAction?.Invoke(Player1, serverAction);
                else
                    OnPlayerAction?.Invoke(Player2, serverAction);

            foreach (var serverAction in command.GetActionsForOpponent())
                if (author == Player1)
                    OnPlayerAction?.Invoke(Player2, serverAction);
                else
                    OnPlayerAction?.Invoke(Player1, serverAction);
        }

        public void EndGame(EndServerBattleReasons reason){
            // Console.WriteLine("");
        } 
    }
}