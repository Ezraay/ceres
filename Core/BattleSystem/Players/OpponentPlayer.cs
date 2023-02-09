namespace Ceres.Core.BattleSystem
{
    public class OpponentPlayer
    {
        public HiddenMultiCardSlot Hand { get; } = new HiddenMultiCardSlot();
        public HiddenMultiCardSlot Pile { get; }
        public MultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public MultiCardSlot Damage { get; } = new MultiCardSlot();
        public MultiCardSlot Defense { get; } = new MultiCardSlot();
        public UnitSlot Champion { get; } = new UnitSlot();
        public UnitSlot LeftUnit { get; } = new UnitSlot();
        public UnitSlot RightUnit { get; } = new UnitSlot();
        public UnitSlot LeftSupport { get; } = new UnitSlot();
        public UnitSlot RightSupport { get; } = new UnitSlot();
        public UnitSlot ChampionSupport { get; } = new UnitSlot();

        public OpponentPlayer(int pileCount)
        {
            Pile = new HiddenMultiCardSlot(pileCount);
        }
    }
}