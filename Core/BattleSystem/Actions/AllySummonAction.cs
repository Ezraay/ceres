using System;

namespace Ceres.Core.BattleSystem
{
    public class AllySummonAction : IServerAction
    {
        public readonly Guid CardId;
        public readonly Guid PlayerId;
        public readonly MultiCardSlotType SlotType;
        public readonly int X;
        public readonly int Y;

        public AllySummonAction(Guid playerId, MultiCardSlotType slotType, int x, int y, Guid cardId)
        {
            this.PlayerId = playerId;
            SlotType = slotType;
            Y = y;
            X = x;
            CardId = cardId;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer player = battle.TeamManager.GetPlayer(PlayerId);
            
            UnitSlot unitSlot = player.GetUnitSlot(X, Y);
            IMultiCardSlot multiSlot = player.GetMultiCardSlot(SlotType);
            Card card = multiSlot.GetCard(CardId);

            unitSlot.SetCard(card);
            multiSlot.RemoveCard(card);
        }
    }
}