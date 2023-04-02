using System;
using System.Collections.Generic;
using System.Linq;
using Ceres.Core.Utility.Json;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem.Battles
{
    public class TeamManager : BattleActionCaller
    {
        [JsonProperty] private List<BattleTeam> teams = new List<BattleTeam>();
        [JsonProperty] private SerializableDictionary<Guid, List<Guid>> allies = new SerializableDictionary<Guid, List<Guid>>();

        public BattleTeam CreateTeam()
        {
            BattleTeam team = new BattleTeam();
            teams.Add(team);
            allies.Add(team.Id, new List<Guid>());
            return team;
        }

        private BattleTeam AddExistingTeam(Guid id)
        {
            BattleTeam team = new BattleTeam(id);
            teams.Add(team);
            allies.Add(team.Id, new List<Guid>());
            return team;
        }
        
        public void AddPlayer(IPlayer player, BattleTeam team)
        {
            team.AddPlayer(player);
        }
        
        public void RemovePlayer(IPlayer player)
        {
            BattleTeam? team = GetPlayerTeam(player.Id);

            if (team == null)
                return;
            
            team.RemovePlayer(player);
            if (team.PlayerCount == 0)
            {
                teams.Remove(team);
                foreach (BattleTeam ally in GetAllies(team))
                    allies[ally.Id].Remove(team.Id);
                allies.Remove(team.Id);
                
                CheckGameOver();
            }
        }

        private void CheckGameOver()
        {
            BattleTeam first = teams[0];

            for (var i = 1; i < teams.Count; i++)
            {
                var team = teams[i];
                if (!allies[first.Id].Contains(team.Id))
                    return;
            }

            // Game over
            List<BattleTeam> winningTeams = GetAllies(first);
            winningTeams.Add(first);
            CallBattleAction(new EndGameBattleAction(winningTeams, GetEnemies(first)));
        }

        public BattleTeam? GetPlayerTeam(Guid playerId)
        {
            foreach (BattleTeam team in teams)
                if (team.GetPlayer(playerId) != null)
                    return team;
            
            return null;
        }

        public void MakeAllies(BattleTeam team1, BattleTeam team2)
        {
            if (AreAllies(team1, team2))
                return;

            foreach (var team1Ally in allies[team1.Id])
            {
                allies[team2.Id].Add(team1Ally);
            }
            
            foreach (var team2Ally in allies[team2.Id])
            {
                allies[team1.Id].Add(team2Ally);
            }
            
            allies[team1.Id].Add(team2.Id);
            allies[team2.Id].Add(team1.Id);
        }

        public IPlayer? GetPlayer(Guid playerId)
        {
            foreach (BattleTeam team in teams)
            {
                IPlayer? player = team.GetPlayer(playerId);
                if (player != null)
                    return player;
            }

            return null;
        }

        public List<BattleTeam> GetAllies(BattleTeam team)
        {
            List<BattleTeam> result = new List<BattleTeam>();
            allies.TryGetValue(team.Id, out var allyIds);

            if (allyIds == null)
                return result;
            
            foreach (Guid allyId in allyIds)
            {
                BattleTeam ally = teams.Find(x => x.Id == allyId);
                result.Add(ally);
            }

            return result;
        }

        private bool AreAllies(BattleTeam team1, BattleTeam team2)
        {
            List<BattleTeam> team1Allies = GetAllies(team1);
            List<BattleTeam> team2Allies = GetAllies(team2);
            return team1Allies.Contains(team2) && team2Allies.Contains(team1);
        }

        public List<BattleTeam> GetEnemies(BattleTeam team)
        {
            List<BattleTeam> result = new List<BattleTeam>(teams);
            result.Remove(team);
            
            if (!allies.ContainsKey(team.Id))
                return result;
            List<Guid> allyIds = allies[team.Id];

            foreach (Guid allyId in allyIds)
            {
                result.Remove(result.Find(x => x.Id == allyId));
            }
            
            return result;
        }

        public TeamManager SafeCopy(IPlayer author)
        {
            TeamManager teamManager = new TeamManager();

            foreach (var team in teams)
            {
                List<IPlayer> teamPlayers = team.GetSafeTeam(author);
                BattleTeam newTeam  = teamManager.AddExistingTeam(team.Id);
                foreach (IPlayer player in teamPlayers)
                {
                    newTeam.AddPlayer(player);
                }
            }
            
            foreach (BattleTeam team in teams)
            {
                foreach (BattleTeam ally in GetAllies(team))
                {
                    BattleTeam? team1 = teamManager.GetTeam(team.Id);
                    BattleTeam? team2 = teamManager.GetTeam(ally.Id);
                    if (team2 != null && team1 != null) 
                        teamManager.MakeAllies(team1, team2);
                }
            }
            

            return teamManager;
        }

        private BattleTeam? GetTeam(Guid teamId)
        {
            return teams.FirstOrDefault(x => x.Id == teamId) ?? null;
        }

        public IEnumerable<BattleTeam> GetAllTeams()
        {
            return teams;
        }
    }
}