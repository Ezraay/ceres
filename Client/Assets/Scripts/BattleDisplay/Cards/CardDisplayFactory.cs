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

        public CardDisplay CreateHidden(Vector3 position = new Vector3())
        {
            CardDisplay display = Instantiate(cardDisplay, position, Quaternion.identity);
            display.ShowBack();
            return display;
        }

        public CardDisplay Create(Card card, Vector3 position = new Vector3())
        {
            CardDisplay display = CreateHidden(position);
            display.ShowFront(card);
            displays.Add(card.ID, display);
            return display;
        }

        public void AddManually(CardDisplay display, Card card)
        {
            display.ShowFront(card);
            displays.Add(card.ID, display);
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