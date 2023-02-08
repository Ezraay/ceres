using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseAction : IServerAction
    {
        public void Apply(ClientBattle battle)
        {
            battle.PhaseManager.Advance();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}