using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class AscendCommand : ClientCommand
    {
        [JsonIgnore] private Card? card;

        public Guid CardId;

        public AscendCommand(Guid cardId)
        {
            CardId = cardId;
        }

        public override bool CanExecute(Battle battle, IPlayer author)
        {
            IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
            card = hand.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier < author.Champion.Card.Data.Tier) return false;
            if (card.Data.Tier > author.Champion.Card.Data.Tier + 2) return false;
            if (battle.PhaseManager.Phase != BattlePhase.Ascend) return false;
            return true;
        }

        public override void Apply(ServerBattle battle, IPlayer author)
        {
            MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
            card = hand.GetCard(CardId);

            hand.RemoveCard(card);
            author.Champion.SetCard(card);
            
            battle.AddToStack(new AdvancePhaseCommand(), author, false);
        }

        public override ServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new ServerAction[]
            {
                new AllySummonAction(MultiCardSlotType.Hand, author.Champion.Position, CardId)
            };
        }

        public override ServerAction[] GetActionsForOpponent(IPlayer author)
        {
            if (card == null)
                throw new ArgumentNullException();
            return new ServerAction[]
            {
                new OpponentSummonAction(MultiCardSlotType.Hand, author.Champion.Position, card)
            };
        }
    }
}