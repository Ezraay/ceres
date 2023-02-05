using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class ServerPlayer {
        public string UserName { get; set; } = "";
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public MultiCardSlot Pile { get; } = new MultiCardSlot();

     }
}