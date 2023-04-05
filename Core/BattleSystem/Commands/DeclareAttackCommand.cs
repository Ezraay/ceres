using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackCommand : IClientCommand
	{
		public readonly CardPosition AttackerPosition;
		public readonly Guid TargetPlayerId;
		public readonly CardPosition TargetPosition;

		public DeclareAttackCommand(CardPosition attackerPosition, Guid targetPlayerId, CardPosition targetPosition)
		{
			this.AttackerPosition = attackerPosition;
			this.TargetPlayerId = targetPlayerId;
			this.TargetPosition = targetPosition;
		}

		public bool CanExecute(Battle battle, IPlayer author)
		{
			if (battle.PhaseManager.Phase != BattlePhase.Attack) return false;
			if (battle.PhaseManager.CurrentTurnPlayer != author) return false;
			if (AttackerPosition.Y != 0) return false;

			IPlayer? target = battle.TeamManager.GetPlayer(TargetPlayerId);
			if (target == null) return false;
			
			BattleTeam? myTeam = battle.TeamManager.GetPlayerTeam(author.Id);
			if (myTeam == null) return false;
			
			BattleTeam? targetTeam = battle.TeamManager.GetPlayerTeam(target.Id);
			if (targetTeam == null) return false;
			
			UnitSlot? slot = target.GetUnitSlot(TargetPosition);
			if (targetTeam.ContainsPlayer(author)) return false; // Can't attack own team
			if (battle.TeamManager.AreAllies(myTeam, targetTeam)) return false; // Can't attack ally
			if (slot?.Card == null) return false;
			if (slot.Position.Y != 0) return false;
			return true;
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			IPlayer target = battle.TeamManager.GetPlayer(TargetPlayerId);
			UnitSlot attackerSlot = target?.GetUnitSlot(TargetPosition);
			UnitSlot targetSlot = target?.GetUnitSlot(TargetPosition); 
			battle.CombatManager.AddAttacker(attackerSlot);
			battle.CombatManager.AddTarget(targetSlot, target);
			battle.PhaseManager.Advance();
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new IServerAction[]
			{
				new DeclareAttackAction(author.Id, AttackerPosition, TargetPlayerId, TargetPosition),
				new AdvancePhaseAction()
			};
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			return GetActionsForAlly(author);
		}
	}
}