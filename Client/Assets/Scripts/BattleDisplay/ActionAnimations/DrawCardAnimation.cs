using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : IActionAnimation
    {
        public bool Finished { get; private set; }
        
        [Inject]
        public void Constructor(ICardDatabase cardDatabase)
        {
            Debug.Log(cardDatabase);
        }
        
        public IEnumerator GetEnumerator(IServerAction baseAction, BattleDisplayManager battleDisplayManager)
        {
            DrawCardAction action = (DrawCardAction)baseAction;
            CardDisplay display = CardDisplayFactory.Create();
            display.ShowFront(action.Card);

            yield return battleDisplayManager.player.Hand.AddCard(display);

            Finished = true;
        }
    }
}