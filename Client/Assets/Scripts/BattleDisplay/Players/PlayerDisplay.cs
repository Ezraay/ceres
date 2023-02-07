using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class PlayerDisplay : MonoBehaviour
    {
        [field: SerializeField] public MultiCardSlotDisplay Hand { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Damage { get; private set; }
        [field: SerializeField] public MultiCardSlotDisplay Defense{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay Champion{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftUnit{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightUnit{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay LeftSupport{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay RightSupport{ get; private set; }
        [field: SerializeField] public UnitSlotDisplay ChampionSupport{ get; private set; }
    }
}