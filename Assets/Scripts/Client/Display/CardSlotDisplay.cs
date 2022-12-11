using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client
{
    public class CardSlotDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        private CardDisplay cardDisplay;

        public void Setup(CardSlot slot)
        {
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
        }
    }
}