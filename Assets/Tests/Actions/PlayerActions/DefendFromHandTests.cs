using CardGame;
using NUnit.Framework;
using Tests.Slots;
using UnityEngine;

namespace Tests.Actions.PlayerActions
{
    public class DefendFromHandTests
    {
        private readonly TestCardData testCard = new TestCardData();


        [Test]
        public void CantDefendWhenAttacking()
        {
            Player player = new Player();
            ICard card = player.Hand.AddCard(new Card(testCard));
            Battle battle = TestBattle.CreateTestBattle(player);
            IAction command = new DefendFromHand(card);
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendOutsidePhase()
        {
            Player player = new Player();
            ICard card = player.Hand.AddCard(new Card(testCard));
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            IAction command = new DefendFromHand(card);
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendHigherTier()
        {
            Player player = new Player(null, new Card(testCard));
            ICardData cardData = new TestCardData
            {
                Tier = 3
            };
            ICard card = player.Hand.AddCard(new Card(cardData));
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            IAction command = new DefendFromHand(card);
            battle.Execute(new SetPhase(BattlePhase.Defend));
            battle.Tick();
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendWithNull()
        {
            Player player = new Player();
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            IAction command = new DefendFromHand(null);
            battle.Execute(new SetPhase(BattlePhase.Defend));
            battle.Tick();
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendWithCardNotInHand()
        {
            Player player = new Player();
            ICard card = new Card(testCard);
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            IAction command = new DefendFromHand(card);
            battle.Execute(new SetPhase(BattlePhase.Defend));
            battle.Tick();
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CanDefend()
        {
            Player player = new Player(null, new Card(testCard));
            ICard card = player.Hand.AddCard(new Card(testCard));
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            DefendFromHand command = new DefendFromHand(card);
            battle.Execute(new SetPhase(BattlePhase.Defend));
            battle.Tick();
            Assert.IsTrue(command.CanExecute(battle, player));
        }

        [Test]
        public void Execute()
        {
            Player player = new Player(null, new Card(testCard));
            ICard card = player.Hand.AddCard(new Card(testCard));
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            
            battle.Execute(new SetPhase(BattlePhase.Defend));
            battle.Tick();
            battle.Execute(new DefendFromHand(card), player);
            battle.Tick();
            
            Assert.Contains(card, battle.CombatManager.Defenders.Cards);
        }
    }
}