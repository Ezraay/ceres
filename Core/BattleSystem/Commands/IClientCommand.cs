namespace Ceres.Core.BattleSystem
{
    public interface IClientCommand
    {
        bool CanExecute(Battle battle, IPlayer author);
        void Apply(ServerBattle battle, IPlayer author);
        IServerAction[] GetActionsForAlly(IPlayer author);
        IServerAction[] GetActionsForOpponent(IPlayer author);
    }
}