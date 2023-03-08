using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.BattleDisplay
{
    public class VanguardCardDisplay : CardDisplay
    {
        [SerializeField] private CardSpriteManager cardSpriteManager;
        [SerializeField] private Image sprite;

        public override void ShowFront(Card card)
        {
            base.ShowFront(card);
            
            Sprite image = cardSpriteManager.GetSprite(card.Data.ID);
            
            if (sprite != null)
                sprite.sprite = image;
            sprite.gameObject.SetActive(image != null);
        }
    }
}