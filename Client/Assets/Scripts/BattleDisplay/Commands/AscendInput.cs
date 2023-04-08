using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class AscendInput : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            AscendCommand command = new AscendCommand(data.Card.Card.ID);
            return data.StartSlot == data.PlayerDisplay.Hand &&
                   data.EndSlot == data.PlayerDisplay.Champion && 
                   data.EndSlot.Owner == data.PlayerDisplay &&
                   command.CanExecute(data.ClientBattle, data.MyPlayer);
        }

        public IClientCommand GetCommand(InputCommandData data)
        {
            return new AscendCommand(data.Card.Card.ID);
        }
    }
}