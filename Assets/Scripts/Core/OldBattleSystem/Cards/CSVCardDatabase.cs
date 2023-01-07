using System.Collections.Generic;
using UnityEngine;

namespace Ceres.Core.OldBattleSystem.Cards
{
    public class CSVCardDatabase : ICardDatabase
    {
        private Dictionary<string, ICardData> data = new Dictionary<string, ICardData>();

        public CSVCardDatabase(string csvData, bool skipHeader)
        {
            string[] lines = csvData.Split('\n');
            for (int i = skipHeader ? 1 : 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                Debug.Log(lines[i]);
                ICardData cardData = new CardData(
                    values[0], 
                    values[1], 
                    int.Parse(values[2]), 
                    int.Parse(values[3]), 
                    int.Parse(values[4]));
                data.Add(cardData.ID, cardData);
            }
        }
        
        public ICardData GetCardData(string id)
        {
            data.TryGetValue(id, out var result);
            return result;
        }
    }
}