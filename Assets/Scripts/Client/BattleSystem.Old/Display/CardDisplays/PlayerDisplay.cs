using Ceres.Core.OldBattleSystem.Players;
using UnityEngine;

namespace Ceres.Client.BattleSystem.Old.Display.CardDisplays
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private SingleCardSlotDisplay champion;
        [SerializeField] private MultiCardSlotDisplay hand;
        [SerializeField] private MultiCardSlotDisplay damage;
        
        public void Setup(IPlayer player)
        {
            hand.Setup(player.Hand, player);
            champion.Setup(player.Champion, player);
            damage.Setup(player.Damage, player);
        }
    }
}