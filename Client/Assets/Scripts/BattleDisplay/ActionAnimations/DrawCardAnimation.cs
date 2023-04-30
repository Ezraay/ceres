using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
        {
            DrawCardAction action = (DrawCardAction)baseAction;
            PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.PlayerId);
            CardDisplay display = data.CardDisplayFactory.Create(action.Card);
            display.transform.position = playerDisplay.Pile.position;
            playerDisplay.Hand.AddCard(display);
            yield return playerDisplay.Hand.UpdatePositions();
        }
    }
}