namespace Ceres.Core.BattleSystem
{
    public interface IPlayer
    {
        // public IMultiCardSlot Pile { get; }
        // public IMultiCardSlot Hand { get; }
        // public IMultiCardSlot Graveyard { get; }
        // public IMultiCardSlot Damage { get; }
        // public IMultiCardSlot Defense { get; }
        // public IUnitSlot Champion { get; }

        public int Width { get; }
        public int Height { get; }
        public UnitSlot Champion { get; }
        public void LoadDeck(IDeck deck);
        public UnitSlot GetUnitSlot(int x, int y);
        public IMultiCardSlot GetMultiCardSlot(MultiCardSlotType type);
    }
}