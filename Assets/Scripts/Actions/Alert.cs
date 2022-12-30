﻿namespace CardGame
{
    public class Alert : IAction
    {
        private readonly CardSlot slot;

        public Alert(CardSlot slot)
        {
            this.slot = slot;
        }
        
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return slot != null && 
                   slot.Card != null;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            slot.Alert();
        }
    }
}