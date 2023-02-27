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
    }
}