namespace CardGame
{
    public class Alert : IAction
    {
        private readonly CardSlot slot;

        public Alert(CardSlot slot)
        {
            this.slot = slot;
        }
        
        public bool CanExecute(Battle battle, Player player)
        {
            return slot != null && 
                   slot.Card != null;
        }

        public void Execute(Battle battle, Player player)
        {
            slot.Alert();
        }
    }
}