using System;
using System.Linq;

namespace CardGame
{
    public enum BattlePhase
    {
        Draw,
        Attack,
        Defend,
        Damage
    }

    public class BattlePhaseManager : IEquatable<BattlePhase>
    {
        private static readonly BattlePhase FirstPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
        private static readonly BattlePhase LastPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
        public BattlePhase Value { get; private set; }
        public event Action OnTurnEnd;
        public event Action<BattlePhase> OnChange;

        public void Advance()
        {
            if (Value == LastPhase)
            {
                Value = FirstPhase;
                OnTurnEnd?.Invoke();
            }
            else
            {
                Value++;
            }
            
            OnChange?.Invoke(Value);
        }

        public bool Equals(BattlePhase other)
        {
            return other == Value;
        }
    }
}