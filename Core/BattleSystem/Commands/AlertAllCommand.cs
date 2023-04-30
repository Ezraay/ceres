#region

using Ceres.Core.BattleSystem.Battles;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class AlertAllCommand : ClientCommand
	{
		public override void Apply(ServerBattle battle, IPlayer author)
		{
			for (int x = 0; x < author.Width; x++)
				for (int y = 0; y < author.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					author.GetUnitSlot(position).Alert();
				}
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new ServerAction[] { new AlertAllAction(author.Id) };
		}
	}
}