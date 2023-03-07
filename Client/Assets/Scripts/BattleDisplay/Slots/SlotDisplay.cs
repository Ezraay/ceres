using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public abstract class SlotDisplay : MonoBehaviour
    {
        [SerializeField] protected int sortingOrderOffset;
        
        public Slot Slot { get; private set; }

        public virtual void SetSlot(Slot slot)
        {
            Slot = slot;
        }
    }
}