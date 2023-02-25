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

        public static CardDisplay Create()
        {
            CardDisplay display = Instantiate(instance.cardDisplay, instance.defaultCardLocation.position, Quaternion.identity);
            return display;
        }
    }
}