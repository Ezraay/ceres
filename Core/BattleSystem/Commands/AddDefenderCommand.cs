#region

using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class AddDefenderCommand : ClientCommand
	{
		public readonly Guid CardId;
		[JsonIgnore] private Card? card;

		public AddDefenderCommand(Guid cardId)
		{
			this.CardId = cardId;
		}

		public override bool CanExecute(Battle battle, IPlayer author)
		{
			IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
			this.card = hand.GetCard(this.CardId);
			if (this.card == null) return false;
			if (this.card.Data.Tier > author.Champion.Card.Data.Tier) return false;
			if (battle.PhaseManager.Phase != BattlePhase.Defend) return false;

			if (!battle.CombatManager.ValidAttack) return false;
			// BattleTeam? targetTeam = battle.TeamManager.GetPlayerTeam(battle.CombatManager.TargetPlayer.Id);
			// if (targetTeam == null) return false;
			// if (!targetTeam.ContainsPlayer(author)) return false; // Only players on targets team can defend
			return true;
		}

		public override void Apply(ServerBattle battle, IPlayer author)
		{
			IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
			// card = hand.GetCard(CardId);
			hand.RemoveCard(this.card);
			author.GetMultiCardSlot(MultiCardSlotType.Defense).AddCard(this.card);
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new ServerAction[] { new AllyDefendAction(author.Id, this.CardId) };
		}

		public override ServerAction[] GetActionsForOpponent(IPlayer author)
		{
			if (this.card == null)
				throw new ArgumentNullException();
			return new ServerAction[] { new OpponentDefendAction(author.Id, this.card) };
		}
	}
}