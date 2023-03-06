using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Commands
{
    public interface IInputCommand
    {
        bool CanExecute(InputCommandData data);
        IClientCommand GetCommand(InputCommandData data);
    }
}