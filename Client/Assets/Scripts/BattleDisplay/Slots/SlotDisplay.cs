using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public abstract class SlotDisplay : MonoBehaviour
    {
        [SerializeField] protected int sortingOrderOffset;
        public PlayerDisplay Owner { get; private set; }

        public abstract bool CanDrag(ClientBattle battle, PlayerDisplay myPlayer);
        
        public void SetOwner(PlayerDisplay playerDisplay)
        {
            Owner = playerDisplay;
        }
    }
}