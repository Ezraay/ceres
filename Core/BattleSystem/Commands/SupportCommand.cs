using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
	public class SupportCommand : IClientCommand
	{
		public readonly CardPosition CardPosition;

		public SupportCommand(CardPosition cardPosition)
		{
			this.CardPosition = cardPosition;
		}

		public bool CanExecute(Battle battle, IPlayer author)
		{
			UnitSlot? slot = author.GetUnitSlot(CardPosition);
			UnitSlot? supported = author.GetUnitSlot(new CardPosition(CardPosition.X, CardPosition.Y - 1));
			if (slot == null) return false;
			if (slot.Card == null) return false;
			if (slot.Exhausted) return false;
			if (slot.Card.Data.Tier > 1) return false;
			if (supported == null) return false;
			if (supported.Card == null) return false;
			if (supported.Exhausted) return false;
			if (battle.PhaseManager.Phase != BattlePhase.Attack) return false;
			return true;
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			UnitSlot support = author.GetUnitSlot(CardPosition);
			UnitSlot supported = author.GetUnitSlot(new CardPosition(this.CardPosition.X, this.CardPosition.Y - 1));
			supported.Card.AddAttack(support.Card.Attack);
			support.Exhaust();
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new IServerAction[]
			{
				new SupportUnitAction(author.Id, CardPosition)
			};
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			return GetActionsForAlly(author);
		}
	}
}