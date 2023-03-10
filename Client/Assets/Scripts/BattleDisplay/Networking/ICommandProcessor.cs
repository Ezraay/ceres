using System;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Networking
{
    public interface ICommandProcessor
    {
        public ClientBattle ClientBattle { get; }
        public event Action<IServerAction> OnServerAction;
        public event Action<ClientBattle> OnStartBattle;

        public void ProcessCommand(IClientCommand command);
    }
}