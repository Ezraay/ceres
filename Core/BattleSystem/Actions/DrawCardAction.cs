using System;
using System.Runtime.Serialization;


namespace Ceres.Core.BattleSystem
{
    [Serializable]
    public class DrawCardAction : IServerAction
    {
        public readonly Card Card;

        public DrawCardAction(Card card)
        {
            Card = card;
        }

        public void Apply(ClientBattle battle)
        {
            battle.AllyPlayer.Hand.AddCard(Card);
            battle.AllyPlayer.Pile.RemoveCard();
        }

        // public void GetObjectData(SerializationInfo info, StreamingContext context)
        // {
        //     info.AddValue("Card", Card);
        // }
    }
}