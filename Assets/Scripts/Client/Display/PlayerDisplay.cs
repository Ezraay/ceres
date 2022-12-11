using UnityEngine;

namespace CardGame.Client.Display
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private MultiCardSlotDisplay hand;
        [SerializeField] private CardSlotDisplay champion;
        
        public void Setup(Player player)
        {
            hand.Setup(player.Hand);
            champion.Setup(player.Champion);
        }
    }
}