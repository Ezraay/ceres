using System;

namespace Ceres.Core.BattleSystem
{
	public class AlertAllAction : IServerAction
	{
		public readonly Guid playerId;
		
		public AlertAllAction(Guid playerId)
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
					player.GetUnitSlot(position).Alert();
				}
			}
		}
	}
}