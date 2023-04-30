using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentSummonAction : ServerAction
    {
        public readonly Card Card;
        public readonly MultiCardSlotType SlotType;
        public readonly CardPosition Position;

        public OpponentSummonAction(MultiCardSlotType slotType, CardPosition position, Card card)
        {
            SlotType = slotType;
            this.Position = position;
            Card = card;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer opponent = battle.GetPlayerById(this.AuthorId);

            UnitSlot unitSlot = opponent.GetUnitSlot(Position);
            IMultiCardSlot multiSlot = opponent.GetMultiCardSlot(SlotType);

            unitSlot.SetCard(Card);
            multiSlot.RemoveCard(Card);
        }
    }
}