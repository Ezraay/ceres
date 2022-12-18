using System;
using System.Linq;
using JetBrains.Annotations;

namespace CardGame
{
    

    public class BattlePhaseManager
    {
        public readonly BattlePhase FirstPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
        public readonly BattlePhase LastPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
        public BattlePhase Phase { get; private set; }
        public event Action OnTurnEnd;
        public event Action<BattlePhase> OnPhaseExit;
        public event Action<BattlePhase> OnPhaseEnter;

        public void Set(BattlePhase phase)
        {
            OnPhaseExit?.Invoke(Phase);
            if (phase < Phase) OnTurnEnd?.Invoke();
            Phase = phase;

            OnPhaseEnter?.Invoke(Phase);
        }

        public void Advance()
        {
            OnPhaseExit?.Invoke(Phase);
            if (Phase == LastPhase)
            {
                Phase = FirstPhase;
                OnTurnEnd?.Invoke();
            }
            else
            {
                Phase++;
            }

            OnPhaseEnter?.Invoke(Phase);
        }
    }
}