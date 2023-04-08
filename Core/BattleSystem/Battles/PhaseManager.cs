﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class PhaseManager
    {
        public BattlePhase Phase { get; private set; }
        public IPlayer CurrentTurnPlayer => this.players[this.playerIndex % this.players.Count];

        private readonly BattlePhase FirstPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
        private readonly BattlePhase LastPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
        private List<IPlayer> players;
        private int playerIndex;

        public PhaseManager(BattlePhase? phase = null)
        {
            Phase = phase ?? this.FirstPhase;
        }

        public event Action OnTurnEnd;
        public event Action<BattlePhase> OnPhaseExit;
        public event Action<BattlePhase> OnPhaseEnter;

        public void SetPlayers(List<IPlayer> players)
        {
            this.players = players;
        }
        
        public void Set(BattlePhase phase)
        {
            OnPhaseExit?.Invoke(Phase);
            if (phase <= Phase) OnTurnEnd?.Invoke();
            Phase = phase;

            OnPhaseEnter?.Invoke(Phase);
        }

        public void Advance()
        {
            OnPhaseExit?.Invoke(Phase);
            if (Phase == LastPhase)
            {
                Phase = FirstPhase;
                this.playerIndex++;
                OnTurnEnd?.Invoke();
            }
            else
            {
                Phase++;
            }

            OnPhaseEnter?.Invoke(Phase);
        }

        public PhaseManager Copy()
        {
            PhaseManager result = new PhaseManager(Phase);
            
            return result;
        }
    }
}