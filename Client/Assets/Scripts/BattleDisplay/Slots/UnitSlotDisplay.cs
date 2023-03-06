using System.Collections;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class UnitSlotDisplay : SlotDisplay
    {
        public CardDisplay Card { get; private set; }
        private CardDisplayFactory cardDisplayFactory;
        
        [Inject]
        public void Construct(CardDisplayFactory factory)
        {
            cardDisplayFactory = factory;
        }
        
        public IEnumerator SetCard(CardDisplay display)
        {
            display.transform.parent = transform;
            display.SetParent(this);
            
            // if (Card != null)
            display.SetSortingOrder(1);
            
            display.transform.localRotation = Quaternion.identity;
            yield return display.MoveTo(Vector3.zero);

            
            if (Card != null)
                DestroyCard();
            
            Card = display;
        }

        public void RemoveCard()
        {
            Card.transform.parent = null;
            Card.SetParent(null);
            Card = null;
        }
        
        public void DestroyCard()
        {
            cardDisplayFactory.DestroyDisplay(Card.Card.ID);
            Card = null;
        }
    }
}