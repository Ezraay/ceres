namespace CardGame
{
    public class Card : ICard
    {
        public ICardData Data { get; }

        public Card(CardData data)
        {
            Data = data;
        }
    }
}