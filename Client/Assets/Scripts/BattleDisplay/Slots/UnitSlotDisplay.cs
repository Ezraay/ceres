using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class UnitSlotDisplay : SlotDisplay
    {
        public CardDisplay Card { get; private set; }
        private CardDisplayFactory cardDisplayFactory;
        [SerializeField] private float exhaustSpeed = 10;
        [field: SerializeField] public CardPosition Position { get; private set; } 
        
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
            
            display.transform.localRotation = Quaternion.identity;
            yield return display.MoveTo(Vector3.zero);
            display.SetSortingOrder(sortingOrderOffset);

            
            if (Card != null)
                DestroyCard();
            
            Card = display;
        }

        public void Setup(UnitSlot slot)
        {
            if (slot.Card == null)
                return;

            CardDisplay display = cardDisplayFactory.Create(slot.Card); 
            
            display.transform.parent = transform;
            display.SetParent(this);
            
            display.transform.localRotation = Quaternion.identity;
            display.transform.localPosition = Vector3.zero;
            display.SetSortingOrder(sortingOrderOffset);

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
            // cardDisplayFactory.DestroyDisplay(Card.Card.ID);
            Destroy(Card.gameObject);
            Card = null;
        }

        public IEnumerator Exhaust()
        {
            float distance;
            do
            {
                distance = Card.transform.eulerAngles.z - 90;
                float delta = -this.exhaustSpeed;
                float velocity = Mathf.Min(delta, distance);
                
                Card.transform.Rotate(Vector3.forward * velocity * Time.deltaTime, Space.Self);
                // Vector3 direction = position - transform.localPosition;
                // Vector3 velocity = direction.normalized * movementSpeed;
                // Vector3 delta = velocity * Time.deltaTime * Mathf.Min(distance, 1f);
                
                // transform.localPosition += delta;
                yield return null;
            } while (Card.transform.eulerAngles.z > 270);
        }

        public override bool CanDrag(ClientBattle battle, PlayerDisplay myPlayer)
        {
            return false;
        }
    }
}