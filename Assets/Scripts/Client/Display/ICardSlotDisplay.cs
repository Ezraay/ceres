namespace CardGame
{
    public interface ICardSlotDisplay
    {
        public ISlot Slot { get; }
        public Player Owner { get; }
    }
}