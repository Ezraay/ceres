using System;

namespace Ceres.Core.BattleSystem
{
	public class SupportUnitAction : IServerAction
	{
		public readonly Guid PlayerId;
		public readonly int X;
		public readonly int Y;

		public SupportUnitAction(Guid playerId, int x, int y)
		{
			this.PlayerId = playerId;
			this.X = x;
			this.Y = y;
		}

		public void Apply(ClientBattle battle)
		{
			IPlayer player = battle.TeamManager.GetPlayer(this.PlayerId);
			UnitSlot support = player.GetUnitSlot(this.X, this.Y);
			UnitSlot supported = player.GetUnitSlot(this.X, this.Y - 1);

			support.Exhaust();
			supported.Card.AddAttack(support.Card.Attack);
		}
	}
}