using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class SummonCommand : IClientCommand
    {
        [JsonIgnore] private Card card;
        public Guid CardId;

        public int X;
        public int Y;

        public SummonCommand(int x, int y, Guid cardId)
        {
            X = x;
            Y = y;
            CardId = cardId;
        }

        public bool CanExecute(ClientBattle battle, IPlayer author)
        {
            return GenericCanExecute(battle, author);
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
        {
            return GenericCanExecute(battle, author);
        }

        private bool GenericCanExecute(Battle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier > author.Champion.Card.Data.Tier) return false;
            if (X == 0 && Y == 1) return false;
            return battle.PhaseManager.Phase == BattlePhase.Main;
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand.GetCard(CardId);
            hand.RemoveCard(card);
            author.GetUnitSlot(X, Y).SetCard(card);
        }

        public IServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new IServerAction[] {new AllySummonAction(author.Id, MultiCardSlotType.Hand, X, Y, CardId)};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(author.Id, MultiCardSlotType.Hand, X, Y, card)};
        }
    }
}