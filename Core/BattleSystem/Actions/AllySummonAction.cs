using System;

namespace Ceres.Core.BattleSystem
{
    public class AllySummonAction : IServerAction
    {
        public readonly Guid CardId;
        public readonly Guid PlayerId;
        public readonly MultiCardSlotType SlotType;
        public readonly CardPosition Position;


        public AllySummonAction(Guid playerId, MultiCardSlotType slotType, CardPosition position, Guid cardId)
        {
            this.PlayerId = playerId;
            SlotType = slotType;
            this.Position = position;
            CardId = cardId;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer player = battle.TeamManager.GetPlayer(PlayerId);
            
            UnitSlot unitSlot = player.GetUnitSlot(Position);
            IMultiCardSlot multiSlot = player.GetMultiCardSlot(SlotType);
            Card card = multiSlot.GetCard(CardId);

            unitSlot.SetCard(card);
            multiSlot.RemoveCard(card);
        }
    }
}