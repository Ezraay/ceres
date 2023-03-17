using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class HiddenMultiCardSlot : Slot, IMultiCardSlot
    {
        public int Count { get; private set; }


        public HiddenMultiCardSlot(int count = 0)
        {
            Count = count;
        }

        public void AddCard(Card card = null)
        {
            Count++;
        }

        public void RemoveCard(Card card = null)
        {
            Count--;
        }

        public Card GetCard(Guid cardId)
        {
            return null;
        }
    }
}