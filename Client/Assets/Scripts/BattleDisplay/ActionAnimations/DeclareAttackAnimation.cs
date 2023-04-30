using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class DeclareAttackAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
		{
			DeclareAttackAction action = baseAction as DeclareAttackAction;

			UnitSlotDisplay attacker = data.BattleDisplayManager.GetPlayerDisplay(action.Attacker).GetUnitSlot(action.AttackerPosition);
			yield return attacker.Exhaust();
		}
	}
}