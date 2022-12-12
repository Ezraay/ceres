using System;
using System.Linq;
using JetBrains.Annotations;

namespace CardGame
{
    public enum BattlePhase
    {
        Stand,
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
        public event Action<BattlePhase> OnPhaseEnter;

        public void Set(BattlePhase phase)
        {
            if (phase < Value) OnTurnEnd?.Invoke();
            Value = phase;

            OnPhaseEnter?.Invoke(Value);
        }

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

            OnPhaseEnter?.Invoke(Value);
        }

        public bool Equals(BattlePhase other)
        {
            return other == Value;
        }

        public static bool operator ==([NotNull] BattlePhaseManager obj1, BattlePhase obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(BattlePhaseManager obj1, BattlePhase obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            return Equals((BattlePhase) obj);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }
    }
}