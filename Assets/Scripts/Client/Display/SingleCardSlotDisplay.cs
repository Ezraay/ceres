using UnityEngine;

namespace CardGame
{
    public class SingleCardSlotDisplay : MonoBehaviour, ICardSlotDisplay
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        [SerializeField]private Quaternion alertRotation;
        [SerializeField]private Quaternion exhaustedRotation;
        private BoxCollider boxCollider;
        private CardDisplay cardDisplay;
        public ISlot Slot { get; private set; }
        public IPlayer Owner { get; private set; }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        public void Setup(CardSlot slot, IPlayer player)
        {
            Owner = player;
            Slot = slot;
            slot.OnChange += card => SlotOnOnChange(card, slot.Exhausted);
            slot.OnExhaust += SlotOnOnExhaust;
            slot.OnAlert += SlotOnOnAlert;
            
            SlotOnOnChange(slot.Card, slot.Exhausted);
        }

        private void SlotOnOnAlert()
        {
            cardDisplay.transform.localRotation = alertRotation;
        }

        private void SlotOnOnExhaust()
        {
            cardDisplay.transform.localRotation = exhaustedRotation;
        }

        private void SlotOnOnChange(ICard card, bool exhausted)
        {
            cardDisplay = cardDisplay == null ? Instantiate(cardDisplayPrefab, transform) : cardDisplay;
            if (card != null)
                cardDisplay.Show(card);
            else
                Destroy(cardDisplay.gameObject);
            boxCollider.enabled = card != null;
        }
    }
}