using System;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem.Battles
{
    public class TeamManager
    {
        public readonly List<BattleTeam> AllTeams = new List<BattleTeam>();

        public void AddTeam(BattleTeam team)
        {
            AllTeams.Add(team);
        }

        public BattleTeam GetPlayerTeam(Guid playerId)
        {
            foreach (BattleTeam team in AllTeams)
            {
                if (team.Players.Find(x => x.Id == playerId) != null)
                {
                    return team;
                }
            }
            
            return null;
        }

        public void MakeAllies(BattleTeam team1, BattleTeam team2)
        {
            team1.AddAlly(team2.Id);
            team2.AddAlly(team1.Id);
        }

        public void MakeEnemies(BattleTeam team1, BattleTeam team2)
        {
            team1.AddEnemy(team2.Id);
            team2.AddEnemy(team1.Id);
        }

        public IPlayer GetPlayer(Guid playerId)
        {
            foreach (BattleTeam team in AllTeams)
            {
                foreach (IPlayer player in team.Players)
                {
                    if (player.Id == playerId)
                        return player;
                }
            }

            return null;
        }

        public TeamManager SafeCopy(IPlayer player)
        {
            TeamManager teamManager = new TeamManager();

            foreach (var team in AllTeams)
            {
                BattleTeam newTeam = team.GetSafeTeam(player);
                teamManager.AddTeam(newTeam);
            }

            return teamManager;
        }
    }
}