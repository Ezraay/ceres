using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class CardDisplay : MonoBehaviour
    {
        public static Vector3 CardSize = new Vector3(4, 5, 0.1f);
        
        [SerializeField] private Text title;
        public Card Card { get; private set; }

        public void Show(Card card)
        {
            Card = card;
            title.text = card.Data.Name;
        }
    }
}