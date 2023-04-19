using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class AdvancePhaseAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
		{
			data.BattleHUD.ShowPhase(data.ClientBattle.PhaseManager.Phase);
			yield return null;
		}
	}
}