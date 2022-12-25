using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Battle
    {
        public IPlayer PriorityPlayer => Player1Priority ? Player1 : Player2;
        public IPlayer AttackingPlayer => player1Turn ? Player1 : Player2;
        public IPlayer DefendingPlayer => player1Turn ? Player2 : Player1;
        public bool Player1Priority { get; private set; } = true;
        public readonly CombatManager CombatManager;
        public readonly BattlePhaseManager BattlePhaseManager;
        public readonly IPlayer Player1;
        public readonly IPlayer Player2;
        private bool player1Turn = true;
        private List<(IAction, IPlayer)> actionQueue = new List<(IAction, IPlayer)>();

        public Battle(IPlayer player1, IPlayer player2)
        {
            Player1 = player1;
            Player2 = player2;
            
            player1.PreGameSetup();
            player2.PreGameSetup();

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
                            ExecuteImmediately(new DamageFromPile(), DefendingPlayer);
                        break;
                }
            };
        }

        public void Execute(IAction action, IPlayer author)
        {
            if (action.CanExecute(this, author))
                actionQueue.Add((action, author));
                //action.Execute(this, author);
        }
        
        public void Execute(IAction action)
        {
            Execute(action, PriorityPlayer);
        }

        public void ExecuteImmediately(IAction action, IPlayer author)
        {
            Execute(action, author);
            Tick();
        }

        public void ExecuteImmediately(IAction action)
        {
            ExecuteImmediately(action, PriorityPlayer);
        }

        public void Tick()
        {
            if (actionQueue.Count > 0)
            {
                (IAction action, IPlayer player) = actionQueue[0];
                action.Execute(this, player);
                actionQueue.RemoveAt(0);
                Debug.Log(action);
            }
        }
    }
}