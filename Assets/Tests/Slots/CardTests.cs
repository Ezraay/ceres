using CardGame;
using NUnit.Framework;

namespace Tests.Slots
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