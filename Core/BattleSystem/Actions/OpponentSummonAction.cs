using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentSummonAction : IServerAction
    {
        public readonly Card Card;
        public readonly Guid OpponentId;
        public readonly MultiCardSlotType SlotType;
        public readonly int X;
        public readonly int Y;

        public OpponentSummonAction(Guid opponentId, MultiCardSlotType slotType, int x, int y, Card card)
        {
            OpponentId = opponentId;
            SlotType = slotType;
            Y = y;
            X = x;
            Card = card;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer opponent = battle.TeamManager.GetPlayer(OpponentId);

            UnitSlot unitSlot = opponent.GetUnitSlot(X, Y);
            IMultiCardSlot multiSlot = opponent.GetMultiCardSlot(SlotType);

            unitSlot.SetCard(Card);
            multiSlot.RemoveCard(Card);
        }
    }
}