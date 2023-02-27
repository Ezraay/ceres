using System;

namespace Ceres.Core.BattleSystem
{
    public class AllySummonAction : IServerAction
    {
        public Guid CardId;
        public int X;
        public int Y;

        public AllySummonAction(int x, int y, Guid cardId)
        {
            Y = y;
            X = x;
            CardId = cardId;
        }

        public void Apply(ClientBattle battle)
        {
            UnitSlot slot = battle.AllyPlayer.GetSlotByPosition(X, Y);
            Card card = battle.AllyPlayer.Hand.GetCard(CardId);

            slot.SetCard(card);
            battle.AllyPlayer.Hand.RemoveCard(card);
        }
    }
}