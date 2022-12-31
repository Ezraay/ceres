﻿using System.Collections.Generic;
using System.Linq;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;
using UnityEngine;

namespace Ceres.Client.BattleSystem.Display.CardDisplays
{
    public class MultiCardSlotDisplay : MonoBehaviour, ICardSlotDisplay
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        [SerializeField] private Vector3 cardOffset;
        private BoxCollider boxCollider;
        private readonly Dictionary<ICard, CardDisplay> displays = new Dictionary<ICard, CardDisplay>();
        public ISlot Slot { get; private set; }
        public IPlayer Owner { get; private set; } // TODO: This feels wrong to put here

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        public void Setup(MultiCardSlot slot, IPlayer player)
        {
            Owner = player;
            Slot = slot;
            slot.OnAdd += SlotOnOnAdd;
            slot.OnRemove += SlotOnOnRemove;
            foreach (ICard card in slot.Cards) SlotOnOnAdd(card);
        }

        private void SlotOnOnRemove(ICard card)
        {
            Destroy(displays[card].gameObject);
            displays.Remove(card);

            Reposition();
        }

        private void SlotOnOnAdd(ICard card)
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
            boxCollider.size = new Vector3(cardCount * CardDisplay.CardSize.x, CardDisplay.CardSize.y, CardDisplay.CardSize.z);
            CardDisplay[] cardDisplays = displays.Values.ToArray();
            for (int i = 0; i < cardCount; i++) cardDisplays[i].transform.localPosition = -halfOffset + cardOffset * i;
        }
    }
}