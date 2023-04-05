using System;

namespace Ceres.Core.BattleSystem
{
	public class SupportUnitAction : IServerAction
	{
		public readonly Guid PlayerId;
		public readonly CardPosition Position;

		public SupportUnitAction(Guid playerId, CardPosition position)
		{
			this.PlayerId = playerId;
			this.Position = position;
		}

		public void Apply(ClientBattle battle)
		{
			IPlayer player = battle.TeamManager.GetPlayer(this.PlayerId);
			UnitSlot support = player.GetUnitSlot(Position);
			UnitSlot supported = player.GetUnitSlot(new CardPosition(Position.X, Position.Y - 1));

			support.Exhaust();
			supported.Card.AddAttack(support.Card.Attack);
		}
	}
}