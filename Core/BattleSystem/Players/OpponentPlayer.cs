namespace Ceres.Core.BattleSystem
{
    public class OpponentPlayer
    {
        public HiddenMultiCardSlot Hand { get; } = new HiddenMultiCardSlot();
        public HiddenMultiCardSlot Pile { get; } = new HiddenMultiCardSlot();
    }
}