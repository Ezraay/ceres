using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseAction : ServerAction
    {
        public override void Apply(ClientBattle battle, IPlayer author)
        {
            battle.PhaseManager.Advance();
        }
    }
}