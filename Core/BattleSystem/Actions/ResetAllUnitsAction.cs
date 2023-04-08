using System;

namespace Ceres.Core.BattleSystem
{
	public class ResetAllUnitsAction : IServerAction
	{
		public readonly Guid playerId;
		
		public ResetAllUnitsAction(Guid playerId)
		{
			this.playerId = playerId;
		}
		
		public void Apply(ClientBattle battle)
		{
			IPlayer player = battle.TeamManager.GetPlayer(this.playerId);
			for (int x = 0; x < player.Width; x++)
			{
				for (int y = 0; y < player.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					player.GetUnitSlot(position).Card?.Reset();
				}
			}
		}
	}
}