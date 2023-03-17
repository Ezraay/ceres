namespace Ceres.Core.BattleSystem
{
    public interface IClientCommand
    {
        bool CanExecute(ClientBattle battle, IPlayer author);
        bool CanExecute(ServerBattle battle, IPlayer author);
        void Apply(ServerBattle battle, IPlayer author);
        IServerAction[] GetActionsForAlly(IPlayer author);
        IServerAction[] GetActionsForOpponent(IPlayer author);
    }
}