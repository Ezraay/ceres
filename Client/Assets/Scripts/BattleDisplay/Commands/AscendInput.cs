using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class AscendInput : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            return data.StartSlot == data.PlayerDisplay.Hand &&
                   data.EndSlot == data.PlayerDisplay.Champion;
        }

        public IClientCommand GetCommand(InputCommandData data)
        {
            return new AscendCommand(data.Card.Card.ID);
        }
    }
}