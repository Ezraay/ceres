using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class BattleTeam
    {
        private readonly List<Guid> allies = new List<Guid>();
        private readonly List<Guid> enemies = new List<Guid>();
        public readonly Guid Id;
        public readonly List<IPlayer> Players = new List<IPlayer>();

        public BattleTeam(Guid id, params IPlayer[] players)
        {
            Id = id;
            foreach (IPlayer player in players) AddPlayer(player);
        }

        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
        }

        public void AddEnemy(Guid teamId)
        {
            enemies.Add(teamId);
        }

        public void AddAlly(Guid teamId)
        {
            allies.Add(teamId);
        }

        public bool IsAlly(BattleTeam team)
        {
            return allies.Contains(team.Id);
        }

        public bool IsEnemy(BattleTeam team)
        {
            return enemies.Contains(team.Id);
        }

        public bool ContainsPlayer(IPlayer player)
        {
            return Players.Contains(player);
        }

        public BattleTeam GetSafeTeam(IPlayer player)
        {
            BattleTeam team = new BattleTeam(Id);

            foreach (Guid ally in allies) team.AddAlly(ally);
            foreach (Guid enemy in enemies) team.AddEnemy(enemy);

            foreach (IPlayer otherPlayer in Players)
            {
                IMultiCardSlot hand;
                if (ContainsPlayer(player)) // Same team
                {
                    hand = otherPlayer.GetMultiCardSlot(MultiCardSlotType.Hand);
                }
                else // Separate teams
                {
                    IMultiCardSlot serverHand = otherPlayer.GetMultiCardSlot(MultiCardSlotType.Hand);
                    hand = new HiddenMultiCardSlot(serverHand.Count);
                }

                IMultiCardSlot serverPile = otherPlayer.GetMultiCardSlot(MultiCardSlotType.Pile);
                IMultiCardSlot pile = new HiddenMultiCardSlot(serverPile.Count);
                IPlayer newPlayer = new StandardPlayer(otherPlayer.Id, hand, pile);

                for (int x = 0; x < otherPlayer.Width; x++)
                {
                    for (int y = 0; y < otherPlayer.Height; y++)
                    {
                        newPlayer.GetUnitSlot(x, y).SetCard(otherPlayer.GetUnitSlot(x, y).Card);
                    }
                }
                
                team.AddPlayer(newPlayer);
            }

            return team;
        }
    }
}