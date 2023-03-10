using System;

namespace Ceres.Core.BattleSystem
{
    public class AllySummonAction : IServerAction
    {
        public Guid CardId;
        public MultiCardSlotType SlotType;
        public int X;
        public int Y;

        public AllySummonAction(MultiCardSlotType slotType, int x, int y, Guid cardId)
        {
            SlotType = slotType;
            Y = y;
            X = x;
            CardId = cardId;
        }

        public void Apply(ClientBattle battle)
        {
            UnitSlot unitSlot = battle.AllyPlayer.GetUnitSlot(X, Y);
            IMultiCardSlot multiSlot = battle.AllyPlayer.GetMultiCardSlot(SlotType);
            Card card = multiSlot.GetCard(CardId);

            unitSlot.SetCard(card);
            multiSlot.RemoveCard(card);
        }
    }
}