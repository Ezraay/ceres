using System.Collections.Generic;
using CardGame;
using NUnit.Framework;
using Tests.Slots;
using UnityEngine;

namespace Tests.Actions
{
    public class DamageFromPileTests
    {
        [Test]
        public void CantExecuteWhenNoCardsInPile()
        {
            Player player = new Player(new List<ICard>(), null);
            DamageFromPile command = new DamageFromPile();

            Assert.IsFalse(command.CanExecute(null, player));
        }

        [Test]
        public void CanExecute()
        {
            ICard card = new Card(new TestCardData());
            Player player = new Player(new List<ICard>() {card}, null);
            DamageFromPile command = new DamageFromPile();
            Assert.IsTrue(command.CanExecute(null, player));
        }
        
        [Test]
        public void ShouldAddDamage()
        {
            ICard card = new Card(new TestCardData());
            List<ICard> pile = new List<ICard>() {card};
            Player player = new Player(pile, null);

            DamageFromPile command = new DamageFromPile();
            command.Execute(null, player);

            Assert.IsTrue(player.Damage.Cards.Count > 0);
            Assert.IsEmpty(player.Pile.Cards);
            Assert.AreEqual(card, player.Damage.Cards[0]);
        }
    }
}