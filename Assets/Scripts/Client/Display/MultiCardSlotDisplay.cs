using System.Collections.Generic;
using System.Linq;
using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client
{
    public class MultiCardSlotDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        [SerializeField] private Vector3 cardOffset;
        private Dictionary<Card, CardDisplay> displays = new Dictionary<Card, CardDisplay>();

        public void Setup(MultiCardSlot slot)
        {
            slot.OnAdd += SlotOnOnAdd;
            slot.OnRemove += SlotOnOnRemove;
            foreach (Card card in slot.Cards)
            {
                SlotOnOnAdd(card);
            }
        }

        private void SlotOnOnRemove(Card card)
        {
            Destroy(displays[card].gameObject);
            displays.Remove(card);
            
            Reposition();
        }

        private void SlotOnOnAdd(Card card)
        {
            CardDisplay cardDisplay =  Instantiate(cardDisplayPrefab, transform);
            cardDisplay.Show(card);
            displays.Add(card, cardDisplay);
            
            Reposition();
        }

        private void Reposition()
        {
            int cardCount = displays.Count;
            Vector3 halfOffset = cardOffset * cardCount / 2;
            CardDisplay[] cardDisplays = displays.Values.ToArray();
            for (int i = 0; i < cardCount; i++)
            {
                cardDisplays[i].transform.position = -halfOffset + cardOffset * i;
            }
        }
    }
}