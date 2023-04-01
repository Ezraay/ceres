using System;

namespace Ceres.Core.BattleSystem.Battles
{
    public class BattleActionCaller
    {
        public event Action<IBattleAction> OnCallAction;

        protected void CallBattleAction(IBattleAction action)
        {
            OnCallAction?.Invoke(action);
        }
    }
}