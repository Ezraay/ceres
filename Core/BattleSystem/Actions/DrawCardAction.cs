using System;

namespace Ceres.Core.BattleSystem
{
    public class DrawCardAction : ServerAction
    {
        public readonly Card Card;
        public readonly Guid PlayerId;

        public DrawCardAction(Guid playerId, Card card)
        {
            PlayerId = playerId;
            Card = card;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer player = battle.GetPlayerById(PlayerId);

            IMultiCardSlot pile = player.GetMultiCardSlot(MultiCardSlotType.Pile);
            IMultiCardSlot hand = player.GetMultiCardSlot(MultiCardSlotType.Hand);

            hand.AddCard(Card);
            pile.RemoveCard(Card);
        }
    }
}