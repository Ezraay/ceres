using System;

namespace Ceres.Core.BattleSystem
{
	public class TakeDamageAction : ServerAction
	{
		public readonly Card Damage;

		public TakeDamageAction(Guid playerId, Card damage)
		{
			this.Damage = damage;
		}

		public override void Apply(ClientBattle battle, IPlayer author)
		{
			author.GetMultiCardSlot(MultiCardSlotType.Pile).RemoveCard(this.Damage);
			author.GetMultiCardSlot(MultiCardSlotType.Damage).AddCard(this.Damage);
		}
	}
}