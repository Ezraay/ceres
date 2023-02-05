using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public interface IActionAnimation
    {
        public bool Finished { get; }

        public IEnumerator GetEnumerator(IServerAction action, BattleDisplayManager battleDisplayManager);
    }
}