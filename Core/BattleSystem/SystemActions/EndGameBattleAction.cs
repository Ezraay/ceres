using System.Collections.Generic;
using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class EndGameBattleAction : IBattleAction
    {
        public readonly List<BattleTeam> WinningTeams;
        public readonly List<BattleTeam> LosingTeams;

        public EndGameBattleAction(List<BattleTeam> winningTeams, List<BattleTeam> losingTeams)
        {
            this.WinningTeams = winningTeams;
            LosingTeams = losingTeams;
        }
    }
}