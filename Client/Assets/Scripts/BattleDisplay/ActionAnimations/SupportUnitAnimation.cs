using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class SupportUnitAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
		{
			SupportUnitAction action = baseAction as SupportUnitAction;

			UnitSlotDisplay attacker = data.BattleDisplayManager.GetPlayerDisplay(action.PlayerId).GetUnitSlot(action.Position);
			yield return attacker.Exhaust();
		}
	}
}