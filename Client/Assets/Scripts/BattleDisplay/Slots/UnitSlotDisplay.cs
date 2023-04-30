using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class UnitSlotDisplay : SlotDisplay
    {
        public CardDisplay Card { get; private set; }
        private CardDisplayFactory cardDisplayFactory;
        [FormerlySerializedAs("exhaustSpeed"),SerializeField] private float rotateSpeed = 10;
        [field: SerializeField] public CardPosition Position { get; private set; } 
        
        private Quaternion alertRotation = Quaternion.identity;
        private Quaternion exhaustedRotation = Quaternion.Euler(0, 0, -90);

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
            if (Card == null) 
                yield break;

            yield return Card.RotateTo(-90f, this.rotateSpeed);
            // while (transform.rotation.z != -90f)
            // {
            //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0, -90f), 3f * Time.deltaTime);
            //     yield return null;
            // }
            // transform.rotation = Quaternion.Euler(0f, 0, -90f);
            // yield return null;
            
            // while (transform.localRotation.z != this.exhaustedRotation.eulerAngles.z)
            // {
            //     transform.localRotation = Quaternion.Slerp(transform.localRotation, this.exhaustedRotation, this.rotateSpeed * Time.deltaTime);
            //     yield return null;
            // }
            //
            // transform.localRotation = this.exhaustedRotation;
            // yield return null;
            
            //
            // float distance;
            // do
            // {
            //     distance = Card.transform.eulerAngles.z - 90;
            //     float delta = -this.exhaustSpeed;
            //     float velocity = Mathf.Min(delta, distance);
            //     
            //     Card.transform.Rotate(Vector3.forward * velocity * Time.deltaTime, Space.Self);
            //     // Vector3 direction = position - transform.localPosition;
            //     // Vector3 velocity = direction.normalized * movementSpeed;
            //     // Vector3 delta = velocity * Time.deltaTime * Mathf.Min(distance, 1f);
            //     
            //     // transform.localPosition += delta;
            //     yield return null;
            // } while (Card.transform.eulerAngles.z > 270);
        }

        public override bool CanDrag(ClientBattle battle, PlayerDisplay myPlayer)
        {
            return false;
        }

        public IEnumerator Alert()
        {
            if (Card == null) 
                yield break;

            yield return Card.RotateTo(0, this.rotateSpeed);
        }
    }
}