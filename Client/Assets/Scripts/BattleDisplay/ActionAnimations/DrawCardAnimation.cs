using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            DrawCardAction action = (DrawCardAction)baseAction;
            PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.PlayerId);
            CardDisplay display = data.CardDisplayFactory.Create(action.Card, playerDisplay.Pile.position);
            playerDisplay.Hand.AddCard(display);
            yield return playerDisplay.Hand.UpdatePositions();
        }
    }
}