﻿using System;
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

        public bool CanExecute(ClientBattle battle)
        {
            return true;
            
            Card card = battle.AllyPlayer.Hand.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier < battle.AllyPlayer.Champion.Card.Data.Tier ||
                card.Data.Tier > battle.AllyPlayer.Champion.Card.Data.Tier + 2) return false;
            return battle.PhaseManager.Phase == BattlePhase.Ascend;
        }

        public bool CanExecute(ServerBattle battle, ServerPlayer author)
        {
            card = author.Hand.GetCard(CardId);

            if (card == null) return false;
            if (card.Data.Tier < author.Champion.Card.Data.Tier ||
                card.Data.Tier > author.Champion.Card.Data.Tier + 2) return false;
            return battle.PhaseManager.Phase == BattlePhase.Ascend;
        }

        public void Apply(ServerBattle battle, ServerPlayer author)
        {
            card = author.Hand.GetCard(CardId);
            author.Hand.RemoveCard(card);
            author.Champion.SetCard(card);
        }

        public IServerAction[] GetActionsForAlly()
        {
            return new IServerAction[] {new AllySummonAction(MultiCardSlotType.Hand, 1, 0, CardId)};
        }

        public IServerAction[] GetActionsForOpponent()
        {
            if (card == null)
                throw new ArgumentNullException();
            return new IServerAction[] {new OpponentSummonAction(MultiCardSlotType.Hand, 1, 0, card)};
        }
    }
}