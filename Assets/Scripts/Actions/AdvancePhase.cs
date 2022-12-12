using System;
using System.Linq;

namespace CardGame.Actions
{
    public class AdvancePhase : IAction
    {
        public bool CanExecute(Battle battle, Player player)
        {
            return true;
        }

        public void Execute(Battle battle, Player player)
        {
            battle.Phase.Advance();
        }
    }
}