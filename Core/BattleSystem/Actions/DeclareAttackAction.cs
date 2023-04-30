using System;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackAction : ServerAction
	{
		public readonly Guid Attacker;
		public readonly CardPosition AttackerPosition;
		public readonly CardPosition TargetPosition;

		public DeclareAttackAction(Guid attacker, CardPosition attackerPosition, CardPosition targetPosition)
		{
			this.Attacker = attacker;
			this.AttackerPosition = attackerPosition;
			this.TargetPosition = targetPosition;
		}

		public override void Apply(ClientBattle battle, IPlayer author)
		{
			IPlayer attacker = battle.GetPlayerById(this.Attacker);
			battle.CombatManager.AddAttacker(attacker.GetUnitSlot(this.AttackerPosition));
			battle.CombatManager.AddTarget(attacker.GetUnitSlot(this.TargetPosition));
			attacker.GetUnitSlot(this.AttackerPosition).Exhaust();
		}
	}
}