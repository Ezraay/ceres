using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDefendAction : IServerAction
    {
        public readonly Guid PlayerId;
        public readonly Card Card;

        public OpponentDefendAction(Guid playerId, Card card)
        {
            Card = card;
            PlayerId = playerId;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer player = battle.TeamManager.GetPlayer(PlayerId);
            player.GetMultiCardSlot(MultiCardSlotType.Hand).RemoveCard(Card);
            player.GetMultiCardSlot(MultiCardSlotType.Defense).AddCard(Card);
        }
    }
}