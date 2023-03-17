using System;
using System.Linq;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class BattlePhaseManager
    {
        public BattlePhase Phase { get; private set; }
        private readonly BattlePhase FirstPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
        private readonly BattlePhase LastPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
        public event Action OnTurnEnd;
        public event Action<BattlePhase> OnPhaseExit;
        public event Action<BattlePhase> OnPhaseEnter;

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