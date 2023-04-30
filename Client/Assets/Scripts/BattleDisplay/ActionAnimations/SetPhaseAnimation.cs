using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class SetPhaseAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
		{
			SetPhaseAction action = baseAction as SetPhaseAction;
			data.BattleHUD.ShowPhase(action.Phase);
			yield return null;
		}
	}
}