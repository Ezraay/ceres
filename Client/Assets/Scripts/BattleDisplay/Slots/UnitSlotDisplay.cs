using System.Collections;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class UnitSlotDisplay : SlotDisplay
    {
        private CardDisplay currentDisplay;
        private CardDisplayFactory cardDisplayFactory;
        
        [Inject]
        public void Construct(CardDisplayFactory factory)
        {
            cardDisplayFactory = factory;
        }
        
        public IEnumerator SetCard(CardDisplay display)
        {
            display.transform.parent = transform;
            
            if (currentDisplay != null)
                display.SetSortingOrder(currentDisplay.SortingOrder + 1);
            
            display.transform.localRotation = Quaternion.identity;
            yield return display.MoveTo(Vector3.zero);

            if (currentDisplay != null)
                RemoveCard();
            
            currentDisplay = display;
        }

        public void RemoveCard()
        {
            cardDisplayFactory.DestroyDisplay(currentDisplay.Card.ID);
        }
    }
}