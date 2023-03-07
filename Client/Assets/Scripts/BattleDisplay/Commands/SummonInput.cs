using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class SummonInput : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            return data.StartSlot == data.PlayerDisplay.Hand &&
                   data.EndSlot.GetType() == typeof(UnitSlotDisplay) && 
                   data.EndSlot != data.PlayerDisplay.Champion && 
                   data.EndSlot.Owner == data.PlayerDisplay;
        }

        public IClientCommand GetCommand(InputCommandData data)
        {
            UnitSlotDisplay display = data.EndSlot as UnitSlotDisplay;
            return new SummonCommand(display.Position.x, display.Position.y, data.Card.Card.ID);
        }
    }
}