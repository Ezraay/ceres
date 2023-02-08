using System;
using System.Runtime.Serialization;

namespace Ceres.Core.BattleSystem
{
    [Serializable]
    public class OpponentDrawCardAction : IServerAction
    {
        public void Apply(ClientBattle battle)
        {
            battle.OpponentPlayer.Hand.AddCard();
            battle.OpponentPlayer.Pile.RemoveCard();
        }

        // public void GetObjectData(SerializationInfo info, StreamingContext context)
        // {
        //     // throw new NotImplementedException();
        //     info.AddValue("OpponentDrawCardAction", 1);
        // }
    }
}