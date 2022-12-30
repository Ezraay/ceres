using System.Collections.Generic;
using CardGame;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions
{
    public class DrawFromPileTests
    {
        [Test]
        public void CantExecuteWhenNoCardsInPile()
        {
            IPlayer player = new Player(new List<ICard>(), null);
            DrawFromPile command = new DrawFromPile();

            Assert.IsFalse(command.CanExecute(null, player));
        }

        [Test]
        public void CanExecute()
        {
            ICard card = new Card(new TestCardData());
            IPlayer player = new Player(new List<ICard>() {card}, null);
            DrawFromPile command = new DrawFromPile();
            Assert.IsTrue(command.CanExecute(null, player));
        }
        
        [Test]
        public void ShouldAddDamage()
        {
            ICard card = new Card(new TestCardData());
            List<ICard> pile = new List<ICard>() {card};
            IPlayer player = new Player(pile, null);

            DrawFromPile command = new DrawFromPile();
            command.Execute(null, player);

            Assert.IsTrue(player.Hand.Cards.Count > 0);
            Assert.IsEmpty(player.Pile.Cards);
            Assert.AreEqual(card, player.Hand.Cards[0]);
        }
    }
}