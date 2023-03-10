namespace Ceres.Core.BattleSystem
{
    public interface IClientCommand
    {
        bool CanExecute(ClientBattle battle);
        bool CanExecute(ServerBattle battle, IPlayer author);
        void Apply(ServerBattle battle, IPlayer author);
        IServerAction[] GetActionsForAlly();
        IServerAction[] GetActionsForOpponent();
    }
}