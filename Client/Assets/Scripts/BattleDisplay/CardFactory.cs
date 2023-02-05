using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardFactory : MonoBehaviour
    {
        private static CSVCardDatabase csvCardDatabase;
        [SerializeField] private TextAsset csvData;

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