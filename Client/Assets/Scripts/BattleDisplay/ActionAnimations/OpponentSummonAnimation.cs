using System.Collections;
using System.Linq;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class OpponentSummonAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            OpponentSummonAction action = baseAction as OpponentSummonAction;

            MultiCardSlotDisplay multiCard = data.OpponentDisplay.GetMultiCardSlot(action.SlotType);
            UnitSlotDisplay slot = data.OpponentDisplay.GetUnitSlot(action.X, action.Y);
            CardDisplay card = multiCard.Displays.First(x => x.IsHidden);
            data.CardDisplayFactory.AddManually(card, action.Card);
            
            multiCard.RemoveCard(card);
            
            var first = StartCoroutine(data, multiCard.UpdatePositions());
            var second = StartCoroutine(data,slot.SetCard(card));
            yield return first;
            yield return second;
        }
    }
}