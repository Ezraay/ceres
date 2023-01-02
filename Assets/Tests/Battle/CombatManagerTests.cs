using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Slots;
using NUnit.Framework;
using Tests.Slots;

namespace Tests
{
    public class CombatManagerTests
    {
        [Test]
        public void AddAttacker()
        {
            CombatManager manager = new CombatManager();
            CardSlot slot = new CardSlot();
            manager.AddAttacker(slot);
            
            Assert.AreEqual(slot, manager.Attacker);
        }
        
        [Test]
        public void AddTarget()
        {
            CombatManager manager = new CombatManager();
            CardSlot slot = new CardSlot();
            manager.AddTarget(slot);
            
            Assert.AreEqual(slot, manager.Target);
        }
        
        [Test]
        public void AddDefender()
        {
            CombatManager manager = new CombatManager();
            Card card = new Card(new TestCardData());
            manager.AddDefender(card);
            
            Assert.Contains(card, manager.Defenders.Cards);
        }

        [Test]
        public void ResetShouldMoveDefenders()
        {
            CombatManager manager = new CombatManager();
            Card card = new Card(new TestCardData());
            MultiCardSlot graveyard = new MultiCardSlot();
            manager.AddDefender(card);
            manager.Reset(graveyard);
            
            Assert.IsEmpty(manager.Defenders.Cards);
            Assert.Contains(card, graveyard.Cards);
        }

        [Test]
        public void ResetShouldNullAttacker()
        {
            CombatManager manager = new CombatManager();
            CardSlot slot = new CardSlot();
            IMultiCardSlot graveyard = new MultiCardSlot();
            manager.AddAttacker(slot);
            manager.Reset(graveyard);
            
            Assert.IsNull(manager.Target);
        }

        [Test]
        public void ResetShouldNullTarget()
        {
            CombatManager manager = new CombatManager();
            CardSlot slot = new CardSlot();
            IMultiCardSlot graveyard = new MultiCardSlot();
            manager.AddTarget(slot);
            manager.Reset(graveyard);
            
            Assert.IsNull(manager.Target);
        }
    }
}