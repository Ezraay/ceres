using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDrawCardAction : IServerAction
    {
        public readonly Guid OpponentId;

        public OpponentDrawCardAction(Guid opponentId)
        {
            OpponentId = opponentId;
        }

        public void Apply(ClientBattle battle)
        {
            IPlayer opponent = battle.TeamManager.GetPlayer(OpponentId);

            IMultiCardSlot hand = opponent.GetMultiCardSlot(MultiCardSlotType.Hand);
            IMultiCardSlot pile = opponent.GetMultiCardSlot(MultiCardSlotType.Pile);

            hand.AddCard(null);
            pile.RemoveCard(null);
        }
    }
}