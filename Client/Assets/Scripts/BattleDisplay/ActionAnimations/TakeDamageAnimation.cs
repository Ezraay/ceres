using System.Collections;
using System.Linq;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class TakeDamageAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
		{
			TakeDamageAction action = (TakeDamageAction)baseAction;
			PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.AuthorId);
			CardDisplay display = data.CardDisplayFactory.Create(action.Damage);
			display.transform.position = playerDisplay.Pile.position;
			playerDisplay.Damage.AddCard(display);
			yield return playerDisplay.Damage.UpdatePositions();
		}
	}
}