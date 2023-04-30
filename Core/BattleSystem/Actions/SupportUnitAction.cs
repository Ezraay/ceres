using System;

namespace Ceres.Core.BattleSystem
{
	public class SupportUnitAction : ServerAction
	{
		// public readonly Guid PlayerId;
		public readonly CardPosition Position;

		public SupportUnitAction(Guid playerId, CardPosition position)
		{
			// this.PlayerId = playerId;
			this.Position = position;
		}

		public override void Apply(ClientBattle battle, IPlayer author)
		{
			IPlayer player = battle.GetPlayerById(this.AuthorId);
			UnitSlot support = player.GetUnitSlot(Position);
			UnitSlot supported = player.GetUnitSlot(new CardPosition(Position.X, Position.Y - 1));

			support.Exhaust();
			supported.Card.AddAttack(support.Card.Attack);
		}
	}
}