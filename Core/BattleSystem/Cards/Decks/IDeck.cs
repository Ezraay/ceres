namespace Ceres.Core.BattleSystem
{

    public interface IDeck
    {
        public ICardData GetChampion();

        public ICardData[] GetPile();
    }
}