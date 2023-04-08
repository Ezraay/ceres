using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class SummonCommand : IClientCommand
    {
        [JsonIgnore] private Card? card;
        public Guid CardId;

        public CardPosition Position;

        public SummonCommand(CardPosition position, Guid cardId)
        {
            Position = position;
            CardId = cardId;
        }

        public bool CanExecute(Battle battle, IPlayer author)
        {
            IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
            card = hand?.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier > author.Champion.Card.Data.Tier) return false;
            if (Position == author.Champion.Position) return false;
            return battle.PhaseManager.Phase == BattlePhase.Main;
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand.GetCard(CardId);
            hand.RemoveCard(card);
            author.GetUnitSlot(Position).SetCard(card);
        }

        public IServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new IServerAction[] {new AllySummonAction(author.Id, MultiCardSlotType.Hand, Position, CardId)};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(author.Id, MultiCardSlotType.Hand, Position, card)};
        }
    }
}