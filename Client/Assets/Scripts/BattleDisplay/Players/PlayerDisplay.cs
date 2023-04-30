using System;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class PlayerDisplay : MonoBehaviour
    {
        private SlotDisplay[] allSlots;

        private UnitSlotDisplay[] unitSlots;
        private MultiCardSlotDisplay[] multiSlots;
        private CardDisplayFactory
            cardFactory;
        [field: SerializeField] public MultiCardSlotDisplay Hand { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Damage { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Defense { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Graveyard { get; private set; }
        [field: SerializeField] public Transform Pile { get; private set; }
        [field: SerializeField] public UnitSlotDisplay Champion { get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftUnit { get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightUnit { get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftSupport { get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightSupport { get; private set; }
        [field: SerializeField] public UnitSlotDisplay ChampionSupport { get; private set; }
        public Guid PlayerId { get; private set; }

        [Inject]
        public void Construct(CardDisplayFactory factory)
        {
            cardFactory = factory;
        }
        
        public MultiCardSlotDisplay GetMultiCardSlot(MultiCardSlotType type)
        {
            return type switch
            {
                MultiCardSlotType.Damage => Damage,
                MultiCardSlotType.Defense => Defense,
                MultiCardSlotType.Hand => Hand,
                MultiCardSlotType.Graveyard => Graveyard,
                _ => null
            };
        }

        public UnitSlotDisplay GetUnitSlot(CardPosition position)
        {
            return position.X switch
            {
                1 => position.Y == 0 ? Champion : ChampionSupport,
                0 => position.Y == 0 ? LeftUnit : LeftSupport,
                2 => position.Y == 0 ? RightUnit : RightSupport,
                _ => null
            };
        }

        public void Setup(IPlayer player)
        {
            PlayerId = player.Id;
            allSlots = new SlotDisplay[]
            {
                Hand, Damage, Defense, Champion, LeftUnit, RightUnit, LeftSupport, RightSupport, ChampionSupport
            };
            unitSlots = new[]
            {
                Champion, LeftUnit, RightUnit, ChampionSupport, LeftSupport, RightSupport
            };
            this.multiSlots = new[]
            {
                Hand, Damage, Defense
            };

            // IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);

            Hand.Setup(player.GetMultiCardSlot(MultiCardSlotType.Hand), cardFactory);
            Damage.Setup(player.GetMultiCardSlot(MultiCardSlotType.Damage), cardFactory);
            Graveyard.Setup(player.GetMultiCardSlot(MultiCardSlotType.Graveyard), cardFactory);
            Defense.Setup(player.GetMultiCardSlot(MultiCardSlotType.Defense), cardFactory);

            foreach (UnitSlotDisplay slot in unitSlots)
                slot.Setup(player.GetUnitSlot(slot.Position));


            // foreach (MultiCardSlotDisplay slot in this.multiSlots)
            // {
            //     slot.Setup(player.GetMultiCardSlot(slot.Type), this.cardFactory);
            // }
            
            foreach (SlotDisplay slot in allSlots) slot.SetOwner(this);
        }

        public class PlayerDisplayFactory : PlaceholderFactory<PlayerDisplay> { }
    }
}