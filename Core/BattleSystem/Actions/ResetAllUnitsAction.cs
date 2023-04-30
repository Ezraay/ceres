using System;

namespace Ceres.Core.BattleSystem
{
	public class ResetAllUnitsAction : ServerAction
	{
		public ResetAllUnitsAction(Guid playerId)
		{
		}
		
		public override void Apply(ClientBattle battle, IPlayer author)
		{
			IPlayer player = battle.GetPlayerById(this.AuthorId);
			for (int x = 0; x < player.Width; x++)
				for (int y = 0; y < player.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					player.GetUnitSlot(position).Card?.Reset();
				}
			
		}
	}
}