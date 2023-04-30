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
			IPlayer player = battle.GetPlayerById(this.AuthorId);

			player.GetMultiCardSlot(MultiCardSlotType.Pile).RemoveCard(this.Damage);
			player.GetMultiCardSlot(MultiCardSlotType.Damage).RemoveCard(this.Damage);
		}
	}
}