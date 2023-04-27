using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.BattleDisplay
{
    public class VanguardCardDisplay : CardDisplay
    {
        [FormerlySerializedAs("cardSpriteManager"),SerializeField] private SpriteCollection spriteCollection;
        [SerializeField] private Image sprite;

        public override void ShowFront(Card card)
        {
            base.ShowFront(card);
            
            Sprite image = this.spriteCollection.GetSprite(card.Data.ID);
            
            if (sprite != null)
                sprite.sprite = image;
            sprite.gameObject.SetActive(image != null);
        }
    }
}