using CardGame;
using NUnit.Framework;

namespace Tests.Slots
{
    public class CardSlotTests
    {
        private ICard testCard = new Card(new TestCardData());

        [Test]
        public void SetCard()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(testCard);
            Assert.AreEqual(slot.Card, testCard);
        }

        [Test]
        public void ClearCard()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(testCard);
            slot.ClearCard();
            Assert.AreEqual(slot.Card, null);
        }

        [Test]
        public void OnCardChange()
        {
            CardSlot slot = new CardSlot();
            ICard card = null;;
            slot.OnChange += newCard => card = newCard;
            slot.SetCard(testCard);
            Assert.AreEqual(testCard, card);
        }

        [Test]
        public void SlotShouldBeAlertOnStart()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(testCard);
            Assert.IsFalse(slot.Exhausted);
        }

        [Test]
        public void Exhaust()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(testCard);
            slot.Exhaust();
            Assert.IsTrue(slot.Exhausted);
        }

        [Test]
        public void Alert()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(testCard);
            slot.Exhaust();
            slot.Alert();
            Assert.False(slot.Exhausted);
        }
    }
}