using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Battle
    {
        public Player PriorityPlayer => Player1Priority ? Player1 : Player2;
        public Player AttackingPlayer => player1Turn ? Player1 : Player2;
        public Player DefendingPlayer => player1Turn ? Player2 : Player1;
        public bool Player1Priority { get; private set; } = true;
        public readonly CombatManager CombatManager;
        public readonly BattlePhaseManager BattlePhaseManager;
        public readonly Player Player1;
        public readonly Player Player2;
        private bool player1Turn = true;
        private List<(IAction, Player)> actionQueue = new List<(IAction, Player)>();

        public Battle(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            CombatManager = new CombatManager();

            BattlePhaseManager = new BattlePhaseManager();
            BattlePhaseManager.OnTurnEnd += () =>
            {
                player1Turn = !player1Turn;
                Player1Priority = player1Turn;
            };

            BattlePhaseManager.OnPhaseExit += phase =>
            {
                switch (phase)
                {
                    case BattlePhase.Damage:
                        CombatManager.Reset(DefendingPlayer.Graveyard);
                        break;
                }
            };

            BattlePhaseManager.OnPhaseEnter += phase =>
            {
                switch (phase)
                {
                    case BattlePhase.Stand:
                        Execute(new Alert(AttackingPlayer.Champion));
                        break;
                    case BattlePhase.Draw:
                        Execute(new DrawFromPile());
                        break;
                    case BattlePhase.Defend:
                        // if (!CombatManager.ValidAttack) Execute(new SetPhase(BattlePhaseManager.LastPhase));
                        // else 
                        Player1Priority = !player1Turn;
                        break;
                    case BattlePhase.Damage:
                        Player1Priority = player1Turn;
                        CombatManager.AddTarget(DefendingPlayer.Champion); // TODO: Let player choose target
                        for (int i = 0; i < CombatManager.DamageCount(); i++)
                            Execute(new DamageFromPile(), DefendingPlayer);
                        break;
                }
            };
        }

        public void Execute(IAction action, Player author)
        {
            Debug.Log(action);
            if (action.CanExecute(this, author))
                actionQueue.Add((action, author));
                //action.Execute(this, author);
        }

        public void Tick()
        {
            if (actionQueue.Count > 0)
            {
                (IAction action, Player player) = actionQueue[0];
                action.Execute(this, player);
                actionQueue.RemoveAt(0);
                Debug.Log(action);
            }
        }

        public void Execute(IAction action)
        {
            Execute(action, PriorityPlayer);
        }
    }
}