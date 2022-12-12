using System;
using CardGame.Client.Display;
using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client
{
    public class SingleCardSlotDisplay : MonoBehaviour, ICardSlotDisplay
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        private BoxCollider boxCollider;
        private CardDisplay cardDisplay;
        public ISlot Slot { get; private set; }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        public void Setup(CardSlot slot)
        {
            Slot = slot;
            slot.OnChange += SlotOnOnChange;
            SlotOnOnChange(slot.Card);
        }

        private void SlotOnOnChange(Card card)
        {
            cardDisplay = cardDisplay == null ? Instantiate(cardDisplayPrefab, transform) : cardDisplay;
            if (card != null)
                cardDisplay.Show(card);
            else
                Destroy(cardDisplay.gameObject);
            boxCollider.enabled = card == null;
        }
    }
}