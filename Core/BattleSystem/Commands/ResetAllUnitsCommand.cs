#region

using Ceres.Core.BattleSystem.Battles;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class ResetAllUnitsCommand : IClientCommand
	{
		public bool CanExecute(Battle battle, IPlayer author)
		{
			return false;
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			for (int x = 0; x < author.Width; x++)
				for (int y = 0; y < author.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					author.GetUnitSlot(position).Card?.Reset();
				}
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new IServerAction[] { new ResetAllUnitsAction(author.Id) };
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			return GetActionsForAlly(author);
		}
	}
}