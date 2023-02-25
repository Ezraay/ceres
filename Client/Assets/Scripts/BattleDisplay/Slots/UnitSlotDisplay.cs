using System.Collections;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class UnitSlotDisplay : MonoBehaviour
    {
        private CardDisplay currentDisplay;
        
        public IEnumerator SetCard(CardDisplay display)
        {
            display.transform.parent = transform;
            yield return display.MoveTo(transform.position);

            
            if (currentDisplay != null)
                RemoveCard();

            currentDisplay = display;
        }

        public void RemoveCard()
        {
            Destroy(currentDisplay);
        }
    }
}