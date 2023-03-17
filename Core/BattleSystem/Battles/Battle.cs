using System;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
    public class Battle
    {
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        public readonly TeamManager TeamManager;

        public bool HasPriority(IPlayer player)
        {
            return true; // TODO
        }
        
        public Battle(TeamManager teamManager)
        {
            TeamManager = teamManager;
        }
        
    }
}