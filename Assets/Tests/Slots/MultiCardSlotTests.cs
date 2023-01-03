using System.Collections.Generic;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Slots;
using NUnit.Framework;

namespace Tests.Slots
{
    public class MultiCardSlotTests
    {
        private ICard testCard1;
        private ICard testCard2;
        
        [SetUp]
        public void Setup()
        {
            testCard1 = new Card(new TestCardData());
            testCard2 = new Card(new TestCardData());
        }

        [Test]
        public void Constructor()
        {
            List<ICard> cards = new List<ICard>() {  testCard1};
            MultiCardSlot slot = new MultiCardSlot(cards);
            Assert.AreEqual(slot.Cards, cards);
        }
        
        [Test]
        public void AddCard()
        {
            MultiCardSlot slot = new MultiCardSlot();
            slot.AddCard(testCard1);
            Assert.AreEqual(slot.Cards[0], testCard1);
        }

        [Test]
        public void RemoveCard()
        {
            MultiCardSlot slot = new MultiCardSlot();
            slot.AddCard(testCard1);
            slot.RemoveCard(testCard1);
            Assert.IsEmpty(slot.Cards);
        }

        [Test]
        public void Clear()
        {
            MultiCardSlot slot = new MultiCardSlot();
            slot.AddCard(testCard1);
            slot.AddCard(testCard2);
            slot.Clear();
            Assert.IsEmpty(slot.Cards);
        }

        [Test]
        public void PopCard()
        {
            MultiCardSlot slot = new MultiCardSlot();
            slot.AddCard(testCard1);
            slot.AddCard(testCard2);
            ICard card = slot.PopCard();
            
            Assert.AreEqual(card, testCard1);
            Assert.AreEqual(slot.Cards[0], testCard2);
        }

        [Test]
        public void OnAddCard()
        {
            ICard card = null;
            MultiCardSlot slot = new MultiCardSlot();
            slot.OnAdd += newCard => card = newCard;
            slot.AddCard(testCard1);
            Assert.AreEqual(card, testCard1);
        }

        [Test]
        public void OnRemoveCard()
        {
            ICard card = null;
            MultiCardSlot slot = new MultiCardSlot();
            slot.OnRemove += newCard => card = newCard;
            slot.AddCard(testCard1);
            slot.RemoveCard(testCard1);
            Assert.AreEqual(card, testCard1);
        }
    }
}