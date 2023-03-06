using System.Collections;
using Ceres.Core.BattleSystem;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class OpponentDrawCardAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            CardDisplay display = data.CardDisplayFactory.CreateHidden(data.OpponentDisplay.Pile.position);

            data.OpponentDisplay.Hand.AddCard(display);
            yield return data.OpponentDisplay.Hand.UpdatePositions();
        }
    }
}