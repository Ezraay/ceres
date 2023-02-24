using System;
using System.Runtime.Serialization;


namespace Ceres.Core.BattleSystem
{
    public class DrawCardAction : IServerAction
    {
        public Card Card;

        // public string test = "dsadsa";
        // public CardData data;
        // public Card card;
        
        public DrawCardAction(Card card)
        {
            // data = new CardData("a", "b", 2, 3, 4);
            Card = card;
            // this.card = new Card(new CardData("a", "b", 2, 3, 4));
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