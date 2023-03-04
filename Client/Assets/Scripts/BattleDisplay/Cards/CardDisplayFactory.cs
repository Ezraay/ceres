using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardDisplayFactory : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private Transform defaultCardLocation;


        public CardDisplay Create()
        {
            CardDisplay display = Instantiate(cardDisplay, defaultCardLocation.position, Quaternion.identity);
            return display;
        }
    }
}