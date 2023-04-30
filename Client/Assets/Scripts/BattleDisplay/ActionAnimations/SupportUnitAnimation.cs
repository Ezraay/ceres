using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class SupportUnitAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
		{
			SupportUnitAction action = baseAction as SupportUnitAction;

			UnitSlotDisplay attacker = data.BattleDisplayManager.GetPlayerDisplay(action.AuthorId).GetUnitSlot(action.Position);
			yield return attacker.Exhaust();
		}
	}
}