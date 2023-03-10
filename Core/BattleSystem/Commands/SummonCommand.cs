using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class SummonCommand : IClientCommand
    {
        [JsonIgnore] private Card card;

        public int X;
        public int Y;
        public Guid CardId;

        public SummonCommand(int x, int y, Guid cardId)
        {
            X = x;
            Y = y;
            CardId = cardId;
        }

        public bool CanExecute(ClientBattle battle)
        {
            return true;
            
            // card = battle.AllyPlayer.Hand.GetCard(CardId);
            //
            // if (card == null) return false;
            // if (card.Data.Tier > battle.AllyPlayer.Champion.Card.Data.Tier) return false;
            // if (X == 0 && Y == 1) return false;
            // return battle.PhaseManager.Phase == BattlePhase.Main;
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
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

        public IServerAction[] GetActionsForAlly()
        {
            return new IServerAction[] {new AllySummonAction(MultiCardSlotType.Hand, X, Y, CardId)};
        }

        public IServerAction[] GetActionsForOpponent()
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(MultiCardSlotType.Hand, X, Y, card)};
        }
    }
}