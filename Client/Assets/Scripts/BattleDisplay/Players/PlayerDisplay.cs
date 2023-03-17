using System;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class PlayerDisplay : MonoBehaviour
    {
        [field: SerializeField] public MultiCardSlotDisplay Hand { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Damage { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Defense{ get; private set; }
        [field: SerializeField] public Transform Pile { get; private set; }
        [field: SerializeField] public UnitSlotDisplay Champion{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftUnit{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightUnit{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftSupport{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightSupport{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay ChampionSupport{ get; private set; }

        private SlotDisplay[] allSlots;

        public MultiCardSlotDisplay GetMultiCardSlot(MultiCardSlotType type)
        {
            return type switch
            {
                MultiCardSlotType.Damage => Damage,
                MultiCardSlotType.Defense => Defense,
                MultiCardSlotType.Hand => Hand, 
                _ => null
            };
        }

        public UnitSlotDisplay GetUnitSlot(int x, int y)
        {
            return x switch
            {
                1 => y == 0 ? Champion : ChampionSupport,
                0 => y == 0 ? LeftUnit : LeftSupport,
                2 => y == 0 ? RightUnit : RightSupport,
                _ => null
            };
        }

        public void Setup(IPlayer player)
        {
            IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);
            if (hand is MultiCardSlot)
            {
                
            }
            
            allSlots = new SlotDisplay[]
            {
                Hand, Damage, Defense, Champion, LeftUnit, RightUnit, LeftSupport, RightSupport, ChampionSupport
            };

            foreach (SlotDisplay slot in allSlots)
            {
                slot.SetOwner(this);
            }
        }
    }
}