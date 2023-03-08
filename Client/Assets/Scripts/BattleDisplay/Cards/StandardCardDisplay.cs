using Ceres.Core.BattleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.BattleDisplay
{
    public class StandardCardDisplay : CardDisplay
    {
        [SerializeField] private CardSpriteManager cardSpriteManager;
        [SerializeField] private Image sprite;
        [SerializeField] private TMP_Text attack;
        [SerializeField] private TMP_Text defense;
        [SerializeField] private GameObject defenseParent;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text tier;

        public override void ShowFront(Card card)
        {
            base.ShowFront(card);
            
            name.text = card.Data.Name;
            attack.text = card.Data.Attack.ToString();
            defense.text = card.Data.Defense.ToString();
            defenseParent.gameObject.SetActive(card.Data.Defense != 0);
            
            tier.text = card.Data.Tier.ToString();
            Sprite image = cardSpriteManager.GetSprite(card.Data.ID);
            
            if (sprite != null)
                sprite.sprite = image;
            sprite.gameObject.SetActive(image != null);
        }
    }
}