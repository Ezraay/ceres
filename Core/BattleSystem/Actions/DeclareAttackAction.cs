using System;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackAction : IServerAction
	{
		public readonly Guid Attacker;
		public readonly int AttackerX;
		public readonly int AttackerY;
		public readonly Guid Defender;
		public readonly int TargetX;
		public readonly int TargetY;

		public DeclareAttackAction(Guid attacker, int attackerX, int attackerY, Guid defender, int targetX, int targetY)
		{
			this.Attacker = attacker;
			this.AttackerX = attackerX;
			this.AttackerY = attackerY;
			this.Defender = defender;
			this.TargetX = targetX;
			this.TargetY = targetY;
		}

		public void Apply(ClientBattle battle)
		{
			IPlayer attacker = battle.TeamManager.GetPlayer(this.Attacker);
			attacker.GetUnitSlot(this.AttackerX, this.AttackerY)
				.Exhaust();
		}
	}
}