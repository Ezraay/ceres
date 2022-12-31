using Ceres.Core.BattleSystem.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Ceres.Client.BattleSystem.Display.CardDisplays
{
    public class CardDisplay : MonoBehaviour
    {
        public static Vector3 CardSize = new Vector3(4, 5, 0.1f);
        
        [SerializeField] private Text title;
        [SerializeField] private Text attackText;
        [SerializeField] private Text defenseText;
        public ICard Card { get; private set; }

        public void Show(ICard card)
        {
            Card = card;
            title.text = card.Data.Name;
            attackText.text = "Attack: " + card.Data.Attack;
            defenseText.text = "Defense: " + card.Data.Defense;
        }
    }
}