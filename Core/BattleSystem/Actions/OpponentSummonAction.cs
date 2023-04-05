using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentSummonAction : IServerAction
    {
        public readonly Card Card;
        public readonly Guid OpponentId;
        public readonly MultiCardSlotType SlotType;
        public readonly CardPosition Position;

        public OpponentSummonAction(Guid opponentId, MultiCardSlotType slotType, CardPosition position, Card card)
        {
            OpponentId = opponentId;
            SlotType = slotType;
            this.Position = position;
            Card = card;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer opponent = battle.TeamManager.GetPlayer(OpponentId);

            UnitSlot unitSlot = opponent.GetUnitSlot(Position);
            IMultiCardSlot multiSlot = opponent.GetMultiCardSlot(SlotType);

            unitSlot.SetCard(Card);
            multiSlot.RemoveCard(Card);
        }
    }
}