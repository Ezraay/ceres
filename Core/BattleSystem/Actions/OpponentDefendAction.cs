using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDefendAction : ServerAction
    {
        public readonly Guid PlayerId;
        public readonly Card Card;

        public OpponentDefendAction(Guid playerId, Card card)
        {
            Card = card;
            PlayerId = playerId;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer player = battle.GetPlayerById(PlayerId);
            player.GetMultiCardSlot(MultiCardSlotType.Hand).RemoveCard(Card);
            player.GetMultiCardSlot(MultiCardSlotType.Defense).AddCard(Card);
        }
    }
}