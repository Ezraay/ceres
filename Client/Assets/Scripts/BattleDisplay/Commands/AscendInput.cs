using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class AscendInput : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            return data.StartSlot == data.MyPlayerDisplay.Hand &&
                   data.EndSlot == data.MyPlayerDisplay.Champion && 
                   data.EndSlot.Owner == data.MyPlayerDisplay;
        }

        public IClientCommand GetCommand(InputCommandData data)
        {
            return new AscendCommand(data.Card.Card.ID);
        }
    }
}