using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private Text title;
        public Card Card { get; private set; }

        public void Show(Card card)
        {
            Card = card;
            title.text = card.Data.Name;
        }
    }
}