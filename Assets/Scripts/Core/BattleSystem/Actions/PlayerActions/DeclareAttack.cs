using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Core.BattleSystem.Actions.PlayerActions
{
    public class DeclareAttack : IAction
    {
        private readonly CardSlot slot;

        public DeclareAttack(CardSlot slot)
        {
            this.slot = slot;
        }
        
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return slot != null && 
                   battle.BattlePhaseManager.Phase == BattlePhase.Attack && 
                   player == battle.PriorityPlayer;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            battle.CombatManager.AddAttacker(slot);
        }
    }
}