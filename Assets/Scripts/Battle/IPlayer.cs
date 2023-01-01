namespace CardGame
{
    public interface IPlayer
    {
        CardSlot Champion { get; }
        MultiCardSlot Pile { get; }
        MultiCardSlot Hand { get; }
        MultiCardSlot Graveyard { get; }
        MultiCardSlot Damage { get; }
        void PreGameSetup();
    }
}