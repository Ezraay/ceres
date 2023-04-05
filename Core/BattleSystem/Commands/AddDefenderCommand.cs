using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
	public class AddDefenderCommand : IClientCommand
	{
		[JsonIgnore] private Card? card;
		public readonly Guid CardId;

		public AddDefenderCommand(Guid cardId)
		{
			CardId = cardId;
		}

		public bool CanExecute(Battle battle, IPlayer author)
		{
			IMultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand);
			card = hand.GetCard(CardId);
			if (card == null) return false;
			if (card.Data.Tier > author.Champion.Card.Data.Tier) return false;
			if (battle.PhaseManager.Phase != BattlePhase.Defend) return false;

			if (!battle.CombatManager.ValidAttack) return false;
			BattleTeam? targetTeam = battle.TeamManager.GetPlayerTeam(battle.CombatManager.TargetPlayer.Id);
			if (targetTeam == null) return false;
			if (!targetTeam.ContainsPlayer(author)) return false; // Only players on targets team can defend
			return true;
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			MultiCardSlot hand = author.GetMultiCardSlot(MultiCardSlotType.Hand) as MultiCardSlot;
			Card card = hand.GetCard(CardId);
			hand.RemoveCard(card);
			author.GetMultiCardSlot(MultiCardSlotType.Defense).AddCard(card);
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new IServerAction[] { new AllyDefendAction(author.Id, CardId)};
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			if (card == null)
				throw new ArgumentNullException();
			return new IServerAction[] {new OpponentDefendAction(author.Id, card)};
		}
	}
}