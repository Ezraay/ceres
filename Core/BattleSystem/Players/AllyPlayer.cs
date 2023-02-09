namespace Ceres.Core.BattleSystem
{
    public class AllyPlayer
    {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public HiddenMultiCardSlot Pile { get; } = new HiddenMultiCardSlot();
        public MultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public MultiCardSlot Damage { get; } = new MultiCardSlot();
        public MultiCardSlot Defense { get; } = new MultiCardSlot();
        public UnitSlot Champion { get; } = new UnitSlot();
        public UnitSlot LeftUnit { get; } = new UnitSlot();
        public UnitSlot RightUnit { get; } = new UnitSlot();
        public UnitSlot LeftSupport { get; } = new UnitSlot();
        public UnitSlot RightSupport { get; } = new UnitSlot();
        public UnitSlot ChampionSupport { get; } = new UnitSlot();

        public AllyPlayer(Card champion, int pileCount)
        {
            Champion.SetCard(champion);
            Pile = new HiddenMultiCardSlot(pileCount);
        }
    }
}