#region

using Ceres.Core.BattleSystem.Battles;

#endregion

namespace Ceres.Core.BattleSystem
{
	public abstract class ClientCommand
	{
		public virtual bool CanExecute(Battle battle, IPlayer author)
		{
			return false;
		}

		public abstract void Apply(ServerBattle battle, IPlayer author);
		public abstract ServerAction[] GetActionsForAlly(IPlayer author);

		public virtual ServerAction[] GetActionsForOpponent(IPlayer author)
		{
			return GetActionsForAlly(author);
		}
	}
}