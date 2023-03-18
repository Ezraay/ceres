using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using Zenject;

namespace CardGame.BattleDisplay
{
    public class CardDisplayFactory : PlaceholderFactory<Card, CardDisplay>
    {
        private CardDisplay cardDisplay;
        private DiContainer diContainer;
        private readonly Dictionary<Guid, CardDisplay> displays = new();

        [Inject]
        private void Construct(DiContainer container, CardDisplay cardDisplayPrefab)
        {
            diContainer = container;
            cardDisplay = cardDisplayPrefab;
        }

        public CardDisplay CreateHidden()
        {
            CardDisplay display = diContainer.InstantiatePrefab(cardDisplay).GetComponent<CardDisplay>();
            display.ShowBack();
            display.OnShowFront += card => { display.OnDestroyed += () => DestroyDisplay(card.ID); };
            return display;
        }

        public override CardDisplay Create(Card card)
        {
            CardDisplay display = CreateHidden();
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

        private void DestroyDisplay(Guid id)
        {
            displays.Remove(id);
            // CardDisplay display = GetDisplay(id);
            // Destroy(display.gameObject);
        }
    }
}