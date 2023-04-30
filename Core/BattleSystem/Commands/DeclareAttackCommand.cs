using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackCommand : ClientCommand
	{
		public readonly CardPosition AttackerPosition;
		public readonly CardPosition TargetPosition;

		public DeclareAttackCommand(CardPosition attackerPosition, CardPosition targetPosition)
		{
			this.AttackerPosition = attackerPosition;
			this.TargetPosition = targetPosition;
		}

		public override bool CanExecute(Battle battle, IPlayer author)
		{
			if (battle.PhaseManager.Phase != BattlePhase.Attack) return false;
			if (battle.PhaseManager.CurrentTurnPlayer != author) return false;
			if (AttackerPosition.Y != 0) return false;

			IPlayer opponent = battle.GetEnemy(author);
			UnitSlot? slot = opponent.GetUnitSlot(TargetPosition);
			if (slot?.Card == null) return false;
			if (slot.Position.Y != 0) return false;
			return true;
		}

		public override void Apply(ServerBattle battle, IPlayer author)
		{
			IPlayer opponent = battle.GetEnemy(author);
			UnitSlot attackerSlot = author.GetUnitSlot(TargetPosition);
			UnitSlot targetSlot = opponent.GetUnitSlot(TargetPosition); 
			battle.CombatManager.AddAttacker(attackerSlot);
			battle.CombatManager.AddTarget(targetSlot);
			battle.AddToStack(new AdvancePhaseCommand(), author, false);
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new ServerAction[]
			{
				new DeclareAttackAction(author.Id, AttackerPosition, TargetPosition)
			};
		}
	}
}