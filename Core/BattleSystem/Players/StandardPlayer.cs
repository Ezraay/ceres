namespace Ceres.Core.BattleSystem
{
    public class StandardPlayer
    {
        public MultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public MultiCardSlot Damage { get; } = new MultiCardSlot();
        public MultiCardSlot Defense { get; } = new MultiCardSlot();
        public UnitSlot Champion { get; } = new UnitSlot(1, 0);
        public UnitSlot LeftUnit { get; } = new UnitSlot(0, 0);
        public UnitSlot RightUnit { get; } = new UnitSlot(2, 0);
        public UnitSlot LeftSupport { get; } = new UnitSlot(0, 1);
        public UnitSlot RightSupport { get; } = new UnitSlot(2, 1);
        public UnitSlot ChampionSupport { get; } = new UnitSlot(1, 1);

        public UnitSlot GetSlotByPosition(int x, int y)
        {
            return x switch
            {
                1 => y == 0 ? Champion : ChampionSupport,
                0 => y == 0 ? LeftUnit : LeftSupport,
                2 => y == 0 ? RightUnit : RightSupport,
                _ => null
            };
        }
    }
}