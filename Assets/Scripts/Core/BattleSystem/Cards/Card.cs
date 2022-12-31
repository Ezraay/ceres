namespace Ceres.Core.BattleSystem.Cards
{
    public class Card : ICard
    {
        public ICardData Data { get; }

        public Card(ICardData data)
        {
            Data = data;
        }
    }
}