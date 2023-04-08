using System;

namespace Ceres.Core.BattleSystem
{
	public class AlertAction : IServerAction
	{
		public readonly Guid PlayerId;
		public readonly CardPosition Position;

		public AlertAction(Guid playerId, CardPosition position)
		{
			this.PlayerId = playerId;
			this.Position = position;
		}
		
		public void Apply(ClientBattle battle)
		{
			IPlayer player = battle.TeamManager.GetPlayer(this.PlayerId);
			player.GetUnitSlot(this.Position).Alert();
		}
	}
}