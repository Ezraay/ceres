using System.Collections;
using Ceres.Core.BattleSystem;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class OpponentDrawCardAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            CardDisplay display = data.CardDisplayFactory.CreateHidden();

            yield return data.OpponentDisplay.Hand.AddCard(display);
        }
    }
}