using System;
using CardGame.Networking;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Networking
{
    public interface ICommandProcessor
    {
        public event Action<IServerAction> OnServerAction;
        public event Action<BattleStartConditions> OnStartBattle;
        public event Action<EndBattleReason> OnEndBattle;
        public IPlayer MyPlayer { get; }
        public void Start();

        public void ProcessCommand(IClientCommand command);
    }
}