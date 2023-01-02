using System.Collections.Generic;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Slots;
using NUnit.Framework;

namespace Tests.Slots
{
    public class HiddenMultiCardSlotTests
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
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            Assert.AreEqual(0, slot.Count);
        }
        
        [Test]
        public void AddCard()
        {
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.AddCard(testCard1);
            Assert.AreEqual(1, slot.Count);
        }

        [Test]
        public void RemoveCard()
        {
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.AddCard(testCard1);
            slot.RemoveCard(testCard1);
            Assert.AreEqual(0, slot.Count);
        }

        [Test]
        public void Clear()
        {
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.AddCard(testCard1);
            slot.AddCard(testCard2);
            slot.Clear();
            Assert.AreEqual(0, slot.Count);
        }

        [Test]
        public void PopCard()
        {
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.AddCard(testCard1);
            slot.AddCard(testCard2);
            slot.PopCard();
            
            Assert.AreEqual(1, slot.Count);
        }

        [Test]
        public void OnAddCard()
        {
            bool card = false;
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.OnAdd += newCard => card = true;
            slot.AddCard(testCard1);
            Assert.IsTrue(card);
        }

        [Test]
        public void OnRemoveCard()
        {
            bool card = false;
            HiddenMultiCardSlot slot = new HiddenMultiCardSlot();
            slot.OnRemove += newCard => card = true;
            slot.AddCard(testCard1);
            slot.RemoveCard(testCard1);
            Assert.IsTrue(card);
        }
    }
}