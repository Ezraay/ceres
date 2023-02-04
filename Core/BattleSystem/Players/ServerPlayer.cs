namespace Ceres.Core.BattleSystem
{
    public class ServerPlayer
    {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public MultiCardSlot Pile { get; } = new MultiCardSlot();
    }
}