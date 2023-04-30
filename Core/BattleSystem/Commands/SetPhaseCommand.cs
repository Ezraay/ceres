using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
	public class SetPhaseCommand : ClientCommand
	{
		private readonly BattlePhase phase;

		public SetPhaseCommand(BattlePhase phase)
		{
			this.phase = phase;
		}
		
		public override bool CanExecute(Battle battle, IPlayer author)
		{
			if (battle.PhaseManager.Phase == BattlePhase.Defend) return author != battle.PhaseManager.CurrentTurnPlayer;
			return author == battle.PhaseManager.CurrentTurnPlayer;
		}

		public override void Apply(ServerBattle battle, IPlayer author)
		{
			battle.PhaseManager.Set(this.phase);
		}

		public override ServerAction[] GetActionsForAlly(IPlayer author)
		{
			return new ServerAction[] {new SetPhaseAction(this.phase)};
		}
	}
}