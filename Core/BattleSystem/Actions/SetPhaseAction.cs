using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public class SetPhaseAction : ServerAction
    {
        public readonly BattlePhase Phase;

        public SetPhaseAction(BattlePhase phase)
        {
            this.Phase = phase;
        }
        
        public override void Apply(ClientBattle battle, IPlayer author)
        {
            battle.PhaseManager.Set(this.Phase);
        }
    }
}