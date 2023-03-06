namespace Ceres.Core.BattleSystem
{
    public class AllyPlayer : StandardPlayer
    {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public HiddenMultiCardSlot Pile { get; }

        public AllyPlayer(Card champion, int pileCount)
        {
            Champion.SetCard(champion);
            Pile = new HiddenMultiCardSlot(pileCount);
        }

        public override IMultiCardSlot GetMultiCardSlot(MultiCardSlotType type)
        {
            return type switch
            {
                MultiCardSlotType.Hand => Hand,
                MultiCardSlotType.Pile => Pile,
                _ => base.GetMultiCardSlot(type)
            };
        }
    }
}