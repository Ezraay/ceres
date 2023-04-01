using System;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    [Serializable]
    public class TestDrawCommand : IClientCommand
    {
        private Card drawnCard;

        public bool CanExecute(ClientBattle battle, IPlayer author)
        {
            return true;
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
        {
            return true;
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            MultiCardSlot pile = author.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;

            drawnCard = pile.PopRandomCard();
            hand.AddCard(drawnCard);
        }

        public IServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new IServerAction[] {new DrawCardAction(author.Id, drawnCard)};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            return new IServerAction[] {new OpponentDrawCardAction(author.Id)};
        }
    }
}