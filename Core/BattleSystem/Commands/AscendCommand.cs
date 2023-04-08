using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class AscendCommand : IClientCommand
    {
        [JsonIgnore] private Card card;

        public Guid CardId;

        public AscendCommand(Guid cardId)
        {
            CardId = cardId;
        }

        public bool CanExecute(Battle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand?.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier < author.Champion.Card.Data.Tier) return false;
            if (card.Data.Tier > author.Champion.Card.Data.Tier + 2) return false;
            if (battle.PhaseManager.Phase != BattlePhase.Ascend) return false;
            return true;
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand.GetCard(CardId);

            hand.RemoveCard(card);
            author.Champion.SetCard(card);
        }

        public IServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new IServerAction[] {new AllySummonAction(author.Id, MultiCardSlotType.Hand, author.Champion.Position, CardId)};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(author.Id, MultiCardSlotType.Hand, author.Champion.Position, card)};
        }
    }
}