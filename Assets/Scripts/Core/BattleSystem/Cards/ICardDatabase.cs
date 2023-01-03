namespace Ceres.Core.BattleSystem.Cards
{
    public interface ICardDatabase
    {
        public ICardData GetCardData(string id);
    }
}