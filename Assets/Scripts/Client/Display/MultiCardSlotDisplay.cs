using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Display;
using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client
{
    public class MultiCardSlotDisplay : MonoBehaviour, ICardSlotDisplay
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        [SerializeField] private Vector3 cardOffset;
        private BoxCollider collider;
        private readonly Dictionary<Card, CardDisplay> displays = new Dictionary<Card, CardDisplay>();
        public ISlot Slot { get; private set; }

        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
        }

        public void Setup(MultiCardSlot slot)
        {
            Slot = slot;
            slot.OnAdd += SlotOnOnAdd;
            slot.OnRemove += SlotOnOnRemove;
            foreach (Card card in slot.Cards) SlotOnOnAdd(card);
        }

        private void SlotOnOnRemove(Card card)
        {
            Destroy(displays[card].gameObject);
            displays.Remove(card);

            Reposition();
        }

        private void SlotOnOnAdd(Card card)
        {
            CardDisplay cardDisplay = Instantiate(cardDisplayPrefab, transform);
            cardDisplay.Show(card);
            displays.Add(card, cardDisplay);

            Reposition();
        }

        private void Reposition()
        {
            int cardCount = displays.Count;
            Vector3 halfOffset = cardOffset * (cardCount - 1) / 2;
            collider.size = new Vector3(cardCount * CardDisplay.CardSize.x, CardDisplay.CardSize.y, CardDisplay.CardSize.z);
            CardDisplay[] cardDisplays = displays.Values.ToArray();
            for (int i = 0; i < cardCount; i++) cardDisplays[i].transform.localPosition = -halfOffset + cardOffset * i;
        }
    }
}