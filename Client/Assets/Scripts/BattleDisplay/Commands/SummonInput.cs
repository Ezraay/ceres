using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class SummonInput : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            return data.StartSlot == data.MyPlayerDisplay.Hand &&
                   data.EndSlot.GetType() == typeof(UnitSlotDisplay) && 
                   data.EndSlot != data.MyPlayerDisplay.Champion && 
                   data.EndSlot.Owner == data.MyPlayerDisplay;
        }

        public ClientCommand GetCommand(InputCommandData data)
        {
            UnitSlotDisplay display = data.EndSlot as UnitSlotDisplay;
            return new SummonCommand(display.Position, data.Card.Card.ID);
        }
    }
}