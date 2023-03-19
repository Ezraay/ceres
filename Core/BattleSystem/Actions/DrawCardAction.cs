using System;

namespace Ceres.Core.BattleSystem
{
    public class DrawCardAction : IServerAction
    {
        public readonly Card Card;
        public readonly Guid PlayerId;

        public DrawCardAction(Guid playerId, Card card)
        {
            PlayerId = playerId;
            Card = card;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer player = battle.TeamManager.GetPlayer(PlayerId);

            IMultiCardSlot pile = player.GetMultiCardSlot(MultiCardSlotType.Pile);
            IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);

            hand.AddCard(Card);
            pile.RemoveCard(Card);
        }
    }
}