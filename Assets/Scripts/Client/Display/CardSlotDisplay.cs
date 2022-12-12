using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client.Display
{
    public interface ICardSlotDisplay
    {
        public ISlot Slot { get; }
    }
}