namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseAction : IServerAction
    {
        public void Apply(ClientBattle battle)
        {
            battle.PhaseManager.Advance();
        }
    }
}