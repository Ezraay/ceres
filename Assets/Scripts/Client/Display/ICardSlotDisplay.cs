namespace CardGame
{
    public interface ICardSlotDisplay
    {
        public ISlot Slot { get; }
        public IPlayer Owner { get; }
    }
}