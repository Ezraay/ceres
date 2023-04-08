using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class InputCommandData
    {
        public SlotDisplay StartSlot;
        public SlotDisplay EndSlot;
        public CardDisplay Card;
        public PlayerDisplay PlayerDisplay;
        public ClientBattle ClientBattle;
        
        public InputCommandData(SlotDisplay startSlot, SlotDisplay endSlot, CardDisplay card, ClientBattle clientBattle, PlayerDisplay playerDisplay, IPlayer myPlayer)
        {
            StartSlot = startSlot;
            EndSlot = endSlot;
            Card = card;
            ClientBattle = clientBattle;
            PlayerDisplay = playerDisplay;
            MyPlayer = myPlayer;
        }

        public IPlayer MyPlayer;
    }
}