using System;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Networking
{
    public interface IBattleManager
    {
        public event Action<IServerAction> OnServerAction;

        public void ProcessCommand(IClientCommand command);
    }
}