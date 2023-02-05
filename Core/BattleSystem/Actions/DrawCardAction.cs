namespace Ceres.Core.BattleSystem
{
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
    }
}