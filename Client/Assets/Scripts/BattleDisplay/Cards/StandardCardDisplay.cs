using Ceres.Core.BattleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.BattleDisplay
{
	public class StandardCardDisplay : CardDisplay
	{
		[SerializeField] private SpriteCollection cardSprites;
		[SerializeField] private SpriteCollection tierSprites;
		[SerializeField] private SpriteCollection attackNumbers;
		[SerializeField] private SpriteCollection defendNumbers;
		[FormerlySerializedAs("sprite"),SerializeField] private Image cardSprite;
		// [SerializeField] private TMP_Text attack;
		// [SerializeField] private TMP_Text defense;
		[SerializeField] private new TMP_Text name;
		[SerializeField] private Image tier;
		[SerializeField] private Image statPrefab;
		[SerializeField] private Transform attackParent;
		[SerializeField] private Transform defenseParent;

		public override void ShowFront(Card card)
		{
			base.ShowFront(card);

			this.name.text = card.Data.Name;
			// this.attack.text = card.Data.Attack.ToString();
			// this.defense.text = card.Data.Defense.ToString();

			this.tier.sprite = this.tierSprites.GetSprite(card.Data.Tier.ToString());
			// if (this.tier != null)
			// 	this.tier.text = card.Data.Tier.ToString();
			this.cardSprite.sprite = this.cardSprites.GetSprite(card.Data.ID);

			ShowStat(card.Data.Attack, this.attackParent, this.attackNumbers);
			ShowStat(card.Data.Defense, this.defenseParent, this.defendNumbers);
			// if (this.sprite != null)
			// 	this.sprite.sprite = image;
			// this.sprite.gameObject.SetActive(image != null);
		}

		private void ShowStat(int stat, Transform parent, SpriteCollection spriteCollection)
		{
			Sprite[] sprites = GetSprites(stat, spriteCollection);

			foreach (Transform child in parent)
				Destroy(child.gameObject);

			for (int i = 0; i < sprites.Length; i++)
			{
				Image newImage = Instantiate(this.statPrefab, parent);
				newImage.sprite = sprites[i];
			}
		}

		private Sprite[] GetSprites(int value, SpriteCollection collection)
		{
			string converted = value.ToString();
			Sprite[] result = new Sprite[converted.Length];

			for (int i = 0; i < converted.Length; i++)
			{
				char character = converted[i];
				result[i] = collection.GetSprite(character.ToString());
			}

			return result;
		}
	}
}