using System;

namespace Ceres.Core.BattleSystem
{
    public class AllySummonAction : ServerAction
    {
        public readonly Guid CardId;
        public readonly MultiCardSlotType SlotType;
        public readonly CardPosition Position;


        public AllySummonAction(MultiCardSlotType slotType, CardPosition position, Guid cardId)
        {
            SlotType = slotType;
            this.Position = position;
            CardId = cardId;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer player = battle.Player1;
            
            UnitSlot unitSlot = player.GetUnitSlot(Position);
            IMultiCardSlot multiSlot = player.GetMultiCardSlot(SlotType);
            Card card = multiSlot.GetCard(CardId);

            unitSlot.SetCard(card);
            multiSlot.RemoveCard(card);
        }
    }
}