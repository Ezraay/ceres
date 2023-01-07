
namespace Ceres.Core.BattleSystem.Slots
{
    public class HiddenMultiCardSlot
    {
        public int Count { get; private set; }


        public void AddCard()
        {
            Count++;
        }

        public void RemoveCard()
        {
            Count--;
        }
    }
}