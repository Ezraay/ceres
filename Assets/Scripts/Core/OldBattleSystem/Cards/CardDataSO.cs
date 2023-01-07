using UnityEngine;

namespace Ceres.Core.OldBattleSystem.Cards
{
    [CreateAssetMenu(menuName = "Create Card", fileName = "New Card", order = 0)]
    public class CardDataSO : ScriptableObject, ICardData
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int Tier { get; private set; }
    }
}