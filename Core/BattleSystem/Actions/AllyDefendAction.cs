using System;

namespace Ceres.Core.BattleSystem
{
    public class AllyDefendAction : ServerAction
    {
        public readonly Guid CardId;


        public AllyDefendAction(Guid playerId, Guid cardId)
        {
            this.CardId = cardId;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer player = battle.Player1;
            
            IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);
            IMultiCardSlot defense = player.GetMultiCardSlot(MultiCardSlotType.Defense);
            Card card = hand.GetCard(CardId);

            hand.RemoveCard(card);
            defense.AddCard(card);
        }
    }
}