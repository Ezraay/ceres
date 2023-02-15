using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardFactory : ICardDatabase
    {
        private static CSVCardDatabase csvCardDatabase;

        private CardFactory(string csvPath)
        {
            TextAsset csvData = Resources.Load<TextAsset>(csvPath);
            csvCardDatabase = new CSVCardDatabase(csvData.text.Trim(), true);
        }

        public ICardData GetCardData(string id)
        {
            return csvCardDatabase.GetCardData(id);
        }
    }
}