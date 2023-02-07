﻿using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : IActionAnimation
    {
        public bool Finished { get; private set; }
        
        
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