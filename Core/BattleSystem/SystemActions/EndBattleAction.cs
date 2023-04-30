using System.Collections.Generic;
using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class EndBattleAction : IBattleAction
    {
        public readonly IPlayer Winner;
        public readonly IPlayer Loser;

        public EndBattleAction(IPlayer winner, IPlayer loser)
        {
            this.Winner = winner;
            this.Loser = loser;
        }
    }
}