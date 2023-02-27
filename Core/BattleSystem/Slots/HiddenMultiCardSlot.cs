namespace Ceres.Core.BattleSystem
{
    public class HiddenMultiCardSlot : Slot
    {
        public int Count { get; private set; }


        public HiddenMultiCardSlot(int count = 0)
        {
            Count = count;
        }

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