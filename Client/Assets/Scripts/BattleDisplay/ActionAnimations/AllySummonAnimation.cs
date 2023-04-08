using System.Collections;
using Ceres.Client.Utility;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class AllySummonAnimation : ActionAnimation
    {
        public override IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data)
        {
            AllySummonAction action = baseAction as AllySummonAction;

            PlayerDisplay playerDisplay = data.BattleDisplayManager.GetPlayerDisplay(action.PlayerId);
            MultiCardSlotDisplay multiCard = playerDisplay.GetMultiCardSlot(action.SlotType);
            UnitSlotDisplay slot = playerDisplay.GetUnitSlot(action.Position);
            CardDisplay card = data.CardDisplayFactory.GetDisplay(action.CardId);
            
            multiCard.RemoveCard(card);
            
            
            var first = StartCoroutine(data, multiCard.UpdatePositions());
            var second = StartCoroutine(data,slot.SetCard(card));
            yield return first;
            yield return second;
            
            // yield return StartCoroutine(data, multiCard.UpdatePositions());
            // yield return slot.SetCard(card);
        }
    }
}