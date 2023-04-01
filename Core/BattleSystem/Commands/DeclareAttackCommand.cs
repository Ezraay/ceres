using System;

namespace Ceres.Core.BattleSystem
{
	public class DeclareAttackCommand : IClientCommand
	{
		public readonly int AttackerX;
		public readonly int AttackerY;
		public readonly Guid TargetPlayerId;
		public readonly int TargetX;
		public readonly int TargetY;

		public DeclareAttackCommand(int attackerX, int attackerY, Guid targetPlayerId, int targetX, int targetY)
		{
			this.AttackerX = attackerX;
			this.AttackerY = attackerY;
			this.TargetPlayerId = targetPlayerId;
			this.TargetX = targetX;
			this.TargetY = targetY;
		}

		public bool CanExecute(ClientBattle battle, IPlayer author)
		{
			return CanExecuteGeneric(battle, author);
		}

		public bool CanExecute(ServerBattle battle, IPlayer author)
		{
			return CanExecuteGeneric(battle, author);
		}

		private bool CanExecuteGeneric(Battle battle, IPlayer author)
		{
			return battle.PhaseManager.Phase == BattlePhase.Attack &&
			       battle.PhaseManager.CurrentTurnPlayer == author; // TODO: Check for false values
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			throw new System.NotImplementedException();
		}
	}
}