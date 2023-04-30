using System;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDrawCardAction : ServerAction
    {
        public readonly Guid OpponentId;

        public OpponentDrawCardAction(Guid opponentId)
        {
            OpponentId = opponentId;
        }

        public override void Apply(ClientBattle battle, IPlayer author)
        {
            IPlayer opponent = battle.GetPlayerById(OpponentId);

            IMultiCardSlot hand = opponent.GetMultiCardSlot(MultiCardSlotType.Hand);
            IMultiCardSlot pile = opponent.GetMultiCardSlot(MultiCardSlotType.Pile);

            hand.AddCard(null);
            pile.RemoveCard(null);
        }
    }
}