namespace Ceres.Core.BattleSystem
{
    public class TestDrawCommand : IClientCommand
    {
        private Card drawnCard;

        public bool CanExecute(ClientBattle battle)
        {
            return true;
        }

        public bool CanExecute(ServerBattle battle, ServerPlayer author)
        {
            return true;
        }

        public void Apply(ServerBattle battle, ServerPlayer author)
        {
            drawnCard = author.Pile.PopCard();
            author.Hand.AddCard(drawnCard);
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