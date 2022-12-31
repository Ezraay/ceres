using Ceres.Core.BattleSystem.Cards;

namespace Tests.Slots
{
    public class TestCardData : ICardData
    {
        public string Name { get; set; } = "Test Card";
        public int Tier { get; set; } = 1;
        public int Attack { get; set; } = 5;
        public int Defense { get; set; } = 5;
    }
}