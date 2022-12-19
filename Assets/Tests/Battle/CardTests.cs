using CardGame;
using NUnit.Framework;
using Tests.Slots;

namespace Tests
{
    public class CardTests
    {
        private ICardData testData = new TestCardData();
        
        [Test]
        public void Constructor()
        {
            Card card = new Card(testData);
            Assert.AreEqual(card.Data, testData);
        }
    }
}