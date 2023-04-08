using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
	public class AlertCommand : IClientCommand
	{
		public readonly CardPosition Position;

		public AlertCommand(CardPosition position)
		{
			this.Position = position;
		}
		
		public bool CanExecute(Battle battle, IPlayer author)
		{
			UnitSlot slot = author.GetUnitSlot(this.Position);
			if (slot?.Card == null) return false;
			return true;
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			UnitSlot slot = author.GetUnitSlot(this.Position);
			slot.Alert();
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new IServerAction[] { };
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			return GetActionsForAlly(author);
		}
	}
}