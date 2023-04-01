using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    public class Battle
    {
        public Guid GameId = Guid.NewGuid();
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        public readonly TeamManager TeamManager;

        public bool HasPriority(IPlayer player)
        {
            return true; // TODO
        }

        protected Battle(TeamManager teamManager)
        {
            TeamManager = teamManager;
        }
        
    }
}