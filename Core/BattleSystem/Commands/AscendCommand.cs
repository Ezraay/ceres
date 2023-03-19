using System;
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

        public bool CanExecute(ClientBattle battle, IPlayer author)
        {
            return true;

            // Card card = battle.AllyPlayer.Hand.GetCard(CardId);

            // if (card == null) return false;
            // if (card.Data.Tier < battle.AllyPlayer.Champion.Card.Data.Tier ||
            //     card.Data.Tier > battle.AllyPlayer.Champion.Card.Data.Tier + 2) return false;
            // return battle.PhaseManager.Phase == BattlePhase.Ascend;
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
        {
            return true;
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;

            card = hand.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier < author.Champion.Card.Data.Tier ||
                card.Data.Tier > author.Champion.Card.Data.Tier + 2) return false;
            return battle.PhaseManager.Phase == BattlePhase.Ascend;
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
            return new IServerAction[] {new AllySummonAction(author.Id, MultiCardSlotType.Hand, 1, 0, CardId)};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(author.Id, MultiCardSlotType.Hand, 1, 0, card)};
        }
    }
}