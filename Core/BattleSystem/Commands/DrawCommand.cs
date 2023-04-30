using System;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    [Serializable]
    public class DrawCommand : ClientCommand
    {
        private Card drawnCard;
        
        public override void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            MultiCardSlot pile = author.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;

            drawnCard = pile.PopRandomCard();
            hand.AddCard(drawnCard);
        }

        public override ServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new ServerAction[] {new DrawCardAction(author.Id, drawnCard)};
        }

        public override ServerAction[] GetActionsForOpponent(IPlayer author)
        {
            return new ServerAction[] {new OpponentDrawCardAction(author.Id)};
        }
    }
}