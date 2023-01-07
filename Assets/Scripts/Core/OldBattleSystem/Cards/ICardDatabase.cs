namespace Ceres.Core.OldBattleSystem.Cards
{
    public interface ICardDatabase
    {
        public ICardData GetCardData(string id);
    }
}