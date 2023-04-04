using System.Collections.Generic;
using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class EndBattleAction : IBattleAction
    {
        public readonly List<BattleTeam> WinningTeams;
        public readonly List<BattleTeam> LosingTeams;

        public EndBattleAction(List<BattleTeam> winningTeams, List<BattleTeam> losingTeams)
        {
            this.WinningTeams = winningTeams;
            LosingTeams = losingTeams;
        }
    }
}