﻿using System;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
    public class CSVCardDatabase : ICardDatabase
    {
        private readonly Dictionary<string, ICardData> data = new Dictionary<string, ICardData>();

        public CSVCardDatabase(string csvData, bool skipHeader = true)
        {
            string[] lines = csvData.Split('\n');
            for (int i = skipHeader ? 1 : 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('|');
                ICardData cardData = new CardData(
                    values[0],
                    values[1],
                    values[2],
                    int.Parse(values[3]),
                    int.Parse(values[4]),
                    int.Parse(values[5]));
                data.Add(cardData.ID, cardData);
            }
        }

        public ICardData GetCardData(string id)
        {
            if (!data.TryGetValue(id, out var result))
                throw new ArgumentException("No such card id: " + id);
            return result;
        }
    }
}