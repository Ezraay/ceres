namespace Ceres.Core.BattleSystem
{
    public interface IClientCommand
    {
        bool CanExecute(ClientBattle battle);
        bool CanExecute(ServerBattle battle, ServerPlayer author);
        void Apply(ServerBattle battle, ServerPlayer author);
        IServerAction[] GetActionsForAlly();
        IServerAction[] GetActionsForOpponent();
    }
}