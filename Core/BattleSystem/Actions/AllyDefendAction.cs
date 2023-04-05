using System;

namespace Ceres.Core.BattleSystem
{
    public class AllyDefendAction : IServerAction
    {
        public readonly Guid CardId;
        public readonly Guid PlayerId;


        public AllyDefendAction(Guid playerId, Guid cardId)
        {
            this.PlayerId = playerId;
            this.CardId = cardId;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer player = battle.TeamManager.GetPlayer(PlayerId);
            
            IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);
            IMultiCardSlot defense = player.GetMultiCardSlot(MultiCardSlotType.Defense);
            Card card = hand.GetCard(CardId);

            hand.RemoveCard(card);
            defense.AddCard(card);
        }
    }
}