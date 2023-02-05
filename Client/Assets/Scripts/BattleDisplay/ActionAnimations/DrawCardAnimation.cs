using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class DrawCardAnimation : IActionAnimation
    {
        public bool Finished { get; }
        
        
        public IEnumerator GetEnumerator(IServerAction baseAction, BattleDisplayManager battleDisplayManager)
        {
            DrawCardAction action = (DrawCardAction)baseAction;
            CardDisplay display = CardDisplayFactory.Create(action.Card);

            yield return battleDisplayManager.AllyPlayer.Hand.AddCard(display);
        }
    }
}