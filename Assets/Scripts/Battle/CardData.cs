using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Create Card", fileName = "New Card", order = 0)]
    public class CardData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int Tier { get; private set; }
    }
}