using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : ActionAnimation
    {
        [Inject]
        public void Constructor(ICardDatabase cardDatabase)
        {
            Debug.Log(cardDatabase);
        }
        
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            DrawCardAction action = (DrawCardAction)baseAction;
            CardDisplay display = data.CardDisplayFactory.Create(action.Card);

            StartCoroutine(data, data.ActionAnimator.ShakeCamera(0.4f, 1f));

            yield return data.PlayerDisplay.Hand.AddCard(display);
        }
    }
}