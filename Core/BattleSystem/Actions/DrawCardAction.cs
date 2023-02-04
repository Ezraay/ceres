namespace Ceres.Core.BattleSystem
{
    public class DrawCardAction : IServerAction
    {
        private readonly Card card;

        public DrawCardAction(Card card)
        {
            this.card = card;
        }
        
        public void Apply(ClientBattle battle)
        {
            battle.AllyPlayer.Hand.AddCard(card);
            battle.AllyPlayer.Pile.RemoveCard();
        }
    }
}