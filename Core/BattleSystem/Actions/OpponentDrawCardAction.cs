using System;
using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDrawCardAction : IServerAction
    {
        public void Apply(ClientBattle battle)
        {
            battle.OpponentPlayer.Hand.AddCard();
            battle.OpponentPlayer.Pile.RemoveCard();
        }
    }
}