using System;

namespace Ceres.Core.BattleSystem
{
	public class TakeDamageAction : IServerAction
	{
		public readonly Card Damage;
		public readonly Guid PlayerId;

		public TakeDamageAction(Guid playerId, Card damage)
		{
			this.PlayerId = playerId;
			this.Damage = damage;
		}

		public void Apply(ClientBattle battle)
		{
			IPlayer player = battle.TeamManager.GetPlayer(this.PlayerId);

			player.GetMultiCardSlot(MultiCardSlotType.Pile).RemoveCard(this.Damage);
			player.GetMultiCardSlot(MultiCardSlotType.Damage).RemoveCard(this.Damage);
		}
	}
}