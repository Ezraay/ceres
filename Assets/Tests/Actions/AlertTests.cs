using CardGame;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions
{
    public class AlertTests
    {
        [Test]
        public void CantAlertNull()
        {
            Alert command = new Alert(null);
            Assert.IsFalse(command.CanExecute(null, null));
        }

        [Test]
        public void CantAlertEmptySlot()
        {
            Alert command = new Alert(new CardSlot());
            Assert.IsFalse(command.CanExecute(null, null));
        }
        
        [Test]
        public void AlertShouldAffectSlot()
        {
            CardSlot slot = new CardSlot();
            slot.SetCard(new Card(new TestCardData()));
            slot.Exhaust();

            Alert command = new Alert(slot);
            command.Execute(null, null);
            
            Assert.IsFalse(slot.Exhausted);
        }
    }
}