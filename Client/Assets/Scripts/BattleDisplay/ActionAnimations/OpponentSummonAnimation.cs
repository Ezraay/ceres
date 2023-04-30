using System.Collections;
using System.Linq;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class OpponentSummonAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(ServerAction baseAction, AnimationData data)
        {
            OpponentSummonAction action = baseAction as OpponentSummonAction;

            PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.OpponentId);
            MultiCardSlotDisplay multiCard = playerDisplay.GetMultiCardSlot(action.SlotType);
            UnitSlotDisplay slot = playerDisplay.GetUnitSlot(action.Position);
            CardDisplay card = multiCard.Displays.First(x => x.IsHidden);
            // data.CardDisplayFactory.AddManually(card, action.Card);
            card.ShowFront(action.Card);
            
            multiCard.RemoveCard(card);
            
            var first = StartCoroutine(data, multiCard.UpdatePositions());
            var second = StartCoroutine(data,slot.SetCard(card));
            yield return first;
            yield return second;
        }
    }
}