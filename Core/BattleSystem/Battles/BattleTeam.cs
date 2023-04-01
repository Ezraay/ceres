using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class BattleTeam
    {
        [JsonProperty] private List<IPlayer> players = new List<IPlayer>();
        public readonly Guid Id;

        public BattleTeam() : this(Guid.NewGuid()) { }

        [JsonConstructor]
        public BattleTeam(Guid id)
        {
            Id = id;
        }

        public int PlayerCount => players.Count;

        public void AddPlayer(IPlayer player)
        {
            players.Add(player);
        }

        public void RemovePlayer(IPlayer player)
        {
            players.Remove(player);
        }

        public bool ContainsPlayer(IPlayer player)
        {
            return players.Contains(player);
        }

        public List<IPlayer> GetSafeTeam(IPlayer player)
        {
            List<IPlayer> team = new List<IPlayer>();

            foreach (IPlayer otherPlayer in players)
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
                
                team.Add(newPlayer);
            }

            return team;
        }

        public IPlayer? GetPlayer(Guid playerId)
        {
            return players.FirstOrDefault(player => player.Id == playerId);
        }

        public IEnumerable<IPlayer> GetAllPlayers()
        {
            return players;
        }
    }
}