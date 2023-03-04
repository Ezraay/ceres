using System.Collections;
using Ceres.Core.BattleSystem;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class OpponentDrawCardAnimation : IActionAnimation
    {
        public bool Finished { get; private set; }

        
        
        public IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            CardDisplay display = data.CardDisplayFactory.Create();

            yield return data.OpponentDisplay.Hand.AddCard(display);

            Finished = true;
        }
    }
}