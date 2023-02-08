using System;
using Ceres.Core.Enums;

namespace Ceres.Core.BattleSystem
{
    public class ServerBattle
    {

        public Guid GameId {get;}
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        public ServerPlayer Player1;
        private bool player1Turn;

        public ServerPlayer Player2;

        // TODO: Listen to these to run Action on client
        public event Action<ServerPlayer, IServerAction> OnPlayerAction;
        // public event Action<IServerAction> OnPlayer1Action;
        // public event Action<IServerAction> OnPlayer2Action;

        public ServerBattle(ServerPlayer player1, ServerPlayer player2)
        {
            GameId = Guid.NewGuid();
            Player1 = player1;
            Player2 = player2;
        }

        public void Start()
        {
            player1Turn = true;
            PhaseManager.OnTurnEnd += () => player1Turn = !player1Turn;
        }

        public bool IsPriorityPlayer(ServerPlayer player)
        {
            switch (PhaseManager.Phase)
            {
                default:
                    return player == Player1 ? player1Turn : !player1Turn;
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