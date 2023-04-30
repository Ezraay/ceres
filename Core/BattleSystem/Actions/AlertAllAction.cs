using System;

namespace Ceres.Core.BattleSystem
{
	public class AlertAllAction : ServerAction
	{
		public readonly Guid playerId;
		
		public AlertAllAction(Guid playerId)
		{
			this.playerId = playerId;
		}
		
		public override void Apply(ClientBattle battle, IPlayer author)
		{
			IPlayer player = battle.GetPlayerById(this.playerId);
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