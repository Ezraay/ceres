using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class ServerPlayer : StandardPlayer {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public MultiCardSlot Pile { get; } = new MultiCardSlot();


        public void LoadDeck(IDeck deck)
        {
            foreach (ICardData cardData in deck.GetPile())
            {
                Pile.AddCard(new Card(cardData));
            }
            
            Champion.SetCard(new Card(deck.GetChampion()));
        }
    }
}