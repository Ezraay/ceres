namespace Ceres.Core.BattleSystem
{
    public class AllyPlayer
    {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public HiddenMultiCardSlot Pile { get; } = new HiddenMultiCardSlot();
    }
}