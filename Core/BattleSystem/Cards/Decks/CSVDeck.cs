using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{

    public class CSVDeck : IDeck
    {
        private readonly ICardData champion;
        private readonly ICardData[] pile;

        public CSVDeck(ICardDatabase database, string csvData, bool skipHeader = true)
        {
            List<ICardData> data = new List<ICardData>();
            string[] lines = csvData.Split('\n');
            int startingLine = skipHeader ? 1 : 0;
            for (int i = startingLine; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('|');
                ICardData cardData = database.GetCardData(values[0]);
                int count = int.Parse(values[1]);

                if (i == startingLine)
                    champion = cardData;
                else
                    for (int j = 0; j < count; j++)
                        data.Add(cardData);
            }


            pile = data.ToArray();
        }

        public ICardData GetChampion()
        {
            return champion;
        }

        public ICardData[] GetPile()
        {
            return pile;
        }
    }
}