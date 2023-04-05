using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public interface IPlayer
    {
        [JsonProperty] public Guid Id { get; }
        [JsonProperty] public int Width { get; }
        [JsonProperty] public int Height { get; }
        [JsonProperty] public UnitSlot Champion { get; }
        public void LoadDeck(IDeck deck);
        public UnitSlot? GetUnitSlot(CardPosition position);
        public IMultiCardSlot GetMultiCardSlot(MultiCardSlotType type);
    }
}