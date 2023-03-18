using System.Collections;
using Ceres.Core.BattleSystem;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class OpponentDrawCardAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            OpponentDrawCardAction action = baseAction as OpponentDrawCardAction;
            PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.OpponentId);
            CardDisplay display = data.CardDisplayFactory.CreateHidden();
            display.transform.position = playerDisplay.Pile.position;
            playerDisplay.Hand.AddCard(display);
            yield return playerDisplay.Hand.UpdatePositions();
        }
    }
}