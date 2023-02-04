namespace Ceres.Core.BattleSystem
{
    public interface ICardDatabase
    {
        public ICardData GetCardData(string id);
    }
}