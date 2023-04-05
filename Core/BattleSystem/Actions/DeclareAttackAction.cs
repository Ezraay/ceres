using System;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackAction : IServerAction
	{
		public readonly Guid Attacker;
		public readonly CardPosition AttackerPosition;
		public readonly Guid Target;
		public readonly CardPosition TargetPosition;

		public DeclareAttackAction(Guid attacker, CardPosition attackerPosition, Guid target, CardPosition targetPosition)
		{
			this.Attacker = attacker;
			this.AttackerPosition = attackerPosition;
			this.Target = target;
			this.TargetPosition = targetPosition;
		}

		public void Apply(ClientBattle battle)
		{
			IPlayer attacker = battle.TeamManager.GetPlayer(this.Attacker);
			attacker.GetUnitSlot(this.AttackerPosition).Exhaust();
		}
	}
}