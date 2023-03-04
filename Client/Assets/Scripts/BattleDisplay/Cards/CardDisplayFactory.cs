using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardDisplayFactory : SerializedMonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private Transform defaultCardLocation;
        [SerializeField, ReadOnly] private Dictionary<Guid, CardDisplay> displays;

        public CardDisplay CreateHidden()
        {
            CardDisplay display = Instantiate(cardDisplay, defaultCardLocation.position, Quaternion.identity);
            display.ShowBack();
            return display;
        }

        public CardDisplay Create(Card card)
        {
            CardDisplay display = CreateHidden();
            display.ShowFront(card);
            displays.Add(card.ID, display);
            return display;
        }

        public CardDisplay GetDisplay(Guid id)
        {
            displays.TryGetValue(id, out var result);
            return result;
        }

        public void DestroyDisplay(Guid id)
        {
            CardDisplay display = GetDisplay(id);
            displays.Remove(id);
            Destroy(display.gameObject);
        }
    }
}