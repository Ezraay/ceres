namespace Ceres.Core.BattleSystem
{
    public class OpponentPlayer : StandardPlayer
    {
        public HiddenMultiCardSlot Hand { get; } = new HiddenMultiCardSlot();
        public HiddenMultiCardSlot Pile { get; }

        public OpponentPlayer(int pileCount)
        {
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