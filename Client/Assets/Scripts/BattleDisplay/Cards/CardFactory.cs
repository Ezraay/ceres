using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private TextAsset csvData;
        private static CSVCardDatabase csvCardDatabase;

        private void Start()
        {
            csvCardDatabase = new CSVCardDatabase(csvData.text.Trim(), true);
        }


        public static ICardData CreateCardData(string id)
        {
            return csvCardDatabase.GetCardData(id);
        }
    }
}