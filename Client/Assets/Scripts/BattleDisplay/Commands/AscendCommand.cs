using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public class AscendCommand : IInputCommand
    {
        public bool CanExecute(InputCommandData data)
        {
            return data.StartSlot == data.PlayerDisplay.Hand && 
                   data.EndSlot == data.PlayerDisplay.Champion;
        }

        public IClientCommand GetCommand(InputCommandData data)
        {
            return new Ceres.Core.BattleSystem.AscendCommand(data.Card.Card.ID);
        }
    }
}