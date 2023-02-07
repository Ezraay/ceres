using System.Collections;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class OpponentDrawCardAnimation : IActionAnimation
    {
        public bool Finished { get; private set; }
        public IEnumerator GetEnumerator(IServerAction baseAction, BattleDisplayManager battleDisplayManager)
        {
            OpponentDrawCardAction action = (OpponentDrawCardAction)baseAction;
            CardDisplay display = CardDisplayFactory.Create();

            yield return battleDisplayManager.opponentPlayer.Hand.AddCard(display);

            Finished = true;
        }
    }
}