#region

using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class SummonCommand : ClientCommand
	{
		[JsonIgnore] private Card? card;
		public Guid CardId;

		public CardPosition Position;

		public SummonCommand(CardPosition position, Guid cardId)
		{
			this.Position = position;
			this.CardId = cardId;
		}

		public override bool CanExecute(Battle battle, IPlayer author)
		{
			IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
			this.card = hand?.GetCard(this.CardId);

			if (this.card == null) return false;
			if (this.card.Data.Tier > author.Champion.Card.Data.Tier) return false;
			if (this.Position == author.Champion.Position) return false;
			return battle.PhaseManager.Phase == BattlePhase.Main;
		}

		public override void Apply(ServerBattle battle, IPlayer author)
		{
			MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
			this.card = hand.GetCard(this.CardId);
			hand.RemoveCard(this.card);
			author.GetUnitSlot(this.Position).SetCard(this.card);
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new ServerAction[]
				{ new AllySummonAction(MultiCardSlotType.Hand, this.Position, this.CardId) };
		}

		public override ServerAction[] GetActionsForOpponent(IPlayer author)
		{
			if (this.card == null)
				throw new ArgumentNullException();
			return new ServerAction[]
				{ new OpponentSummonAction(MultiCardSlotType.Hand, this.Position, this.card) };
		}
	}
}