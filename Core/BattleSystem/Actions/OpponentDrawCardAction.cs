using System;
using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    public class OpponentDrawCardAction : IServerAction
    {
        public void Apply(ClientBattle battle)
        {
            IMultiCardSlot hand = battle.OpponentPlayer.GetMultiCardSlot(MultiCardSlotType.Hand);
            IMultiCardSlot pile = battle.OpponentPlayer.GetMultiCardSlot(MultiCardSlotType.Pile);
            
            hand.AddCard(null);
            pile.RemoveCard(null);
        }
    }
}