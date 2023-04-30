using System;

namespace Ceres.Core.BattleSystem
{
	public class AlertAllAction : ServerAction
	{
		
		public AlertAllAction()
		{
		}
		
		public override void Apply(ClientBattle battle, IPlayer author)
		{
			for (int x = 0; x < author.Width; x++)
			{
				for (int y = 0; y < author.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					author.GetUnitSlot(position).Alert();
				}
			}
		}
	}
}