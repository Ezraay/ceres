using System;

namespace Ceres.Core.BattleSystem
{

    [Serializable]
    public class TestDrawCommand : IClientCommand
    {
        private Card drawnCard;

        public bool CanExecute(ClientBattle battle)
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

        public IServerAction[] GetActionsForAlly()
        {
            return new[] {new DrawCardAction(drawnCard)};
        }

        public IServerAction[] GetActionsForOpponent()
        {
            return new[] {new OpponentDrawCardAction()};
        }
    }
}