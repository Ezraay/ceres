using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
	public class AlertAllAnimation : ActionAnimation
	{
		public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
		{
			PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(baseAction.AuthorId);
			IEnumerator cardRotate = null;
			for (int x = 0; x < playerDisplay.Width; x++)
			{
				for (int y = 0; y < playerDisplay.Height; y++)
				{
					CardPosition position = new CardPosition(x, y);
					cardRotate = playerDisplay.GetUnitSlot(position).Alert();
					StartCoroutine(data, cardRotate);
				}
			}
			
			yield return cardRotate;
		}
	}
}