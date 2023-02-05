using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardDisplayFactory : MonoBehaviour
    {
        private static CardDisplayFactory instance;
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private Transform defaultCardLocation;

        private void Start()
        {
            instance = this;
        }

        public static CardDisplay Create(Card card)
        {
            Debug.Log("Hello");
            return Create(card, instance.defaultCardLocation.position);
        }

        public static CardDisplay Create(Card card, Vector3 position)
        {
            CardDisplay display = Instantiate(instance.cardDisplay, position, Quaternion.identity);
            display.SetCard(card);
            return display;
        }
    }
}