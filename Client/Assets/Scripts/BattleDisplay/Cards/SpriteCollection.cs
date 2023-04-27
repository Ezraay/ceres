using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    [CreateAssetMenu(menuName = "Create Sprite Collection", fileName = "New Sprite Collection", order = 0)]
    public class SpriteCollection : SerializedScriptableObject
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Dictionary<string, Sprite> sprites;

        public Sprite GetSprite(string spriteId)
        {
            sprites.TryGetValue(spriteId, out var result);
            return result == null ? defaultSprite : result;
        }
    }
}