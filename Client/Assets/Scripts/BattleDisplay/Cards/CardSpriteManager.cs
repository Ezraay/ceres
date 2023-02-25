using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    [CreateAssetMenu(menuName = "Create CardSpriteManager", fileName = "CardSpriteManager", order = 0)]
    public class CardSpriteManager : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<string, Sprite> sprites;

        public Sprite GetSprite(string cardID)
        {
            sprites.TryGetValue(cardID, out var result);
            return result;
        }
    }
}