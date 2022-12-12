using UnityEngine;

namespace CardGame
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private MultiCardSlotDisplay hand;
        [SerializeField] private SingleCardSlotDisplay champion;
        
        public void Setup(Player player)
        {
            hand.Setup(player.Hand);
            champion.Setup(player.Champion);
        }
    }
}